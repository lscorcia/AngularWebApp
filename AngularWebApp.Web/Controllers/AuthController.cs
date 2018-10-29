using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Web.Entities;
using AngularWebApp.Web.Authentication;
using AngularWebApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

        private readonly AuthDbContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(AuthDbContext ctx, UserManager<IdentityUser> userManager/*, SignInManager<IdentityUser> signInManager*/)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (user.UserName == "johndoe" && user.Password == "def@123")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.UserName),
                };

                var accessTokenString = GenerateAccessTokenString(claims);
                var refreshTokenString = await NewRefreshToken(user.ClientId, user.UserName, accessTokenString);

                return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(RefreshTokenModel model)
        {
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);

            using (AuthRepository rpAuth = new AuthRepository(_ctx))
            {
                var savedRefreshToken = await rpAuth.FindRefreshToken(model.RefreshToken); // retrieve the refresh token from a data store
                if (savedRefreshToken == null || 
                    savedRefreshToken.ClientId != model.ClientId ||
                    savedRefreshToken.ProtectedTicket != model.AccessToken)
                    return BadRequest("Invalid refresh token");

                await rpAuth.RemoveRefreshToken(savedRefreshToken);

                var accessTokenString = GenerateAccessTokenString(principal.Claims);
                var refreshTokenString = await NewRefreshToken(model.ClientId, principal.Identity.Name, accessTokenString);

                return new ObjectResult(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
            }
        }

        #region Private Helpers
        private async Task<string> NewRefreshToken(string clientId, string username, string accessToken)
        {
            var newRefreshTokenString = GenerateRefreshTokenString();

            var newRefreshToken = new RefreshToken();
            newRefreshToken.Id = newRefreshTokenString;
            newRefreshToken.ClientId = clientId;
            newRefreshToken.Subject = username;
            newRefreshToken.ProtectedTicket = accessToken;
            newRefreshToken.IssuedUtc = DateTime.UtcNow;
            newRefreshToken.ExpiresUtc = DateTime.UtcNow.AddDays(1);
            using (AuthRepository rpAuth = new AuthRepository(_ctx))
            {
                await rpAuth.AddRefreshToken(newRefreshToken);
            }

            return newRefreshTokenString;
        }

        private string GenerateAccessTokenString(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(issuer: "AngularWebApp.Web",
                audience: "AngularWebApp.Web.Client",
                claims: claims, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //... 
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            );

            // The method is called WriteToken but returns a string
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // Here we are saying that we don't care about the token's expiration date

                ValidIssuer = "AngularWebApp.Web",
                ValidAudience = "AngularWebApp.Web.Client",
                IssuerSigningKey = _signingKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        #endregion
    }
}
