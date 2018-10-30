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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtAuthController : ControllerBase
    {
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

        private readonly AuthDbContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<JwtAuthController> _log;

        public JwtAuthController(AuthDbContext ctx, UserManager<IdentityUser> userManager, ILogger<JwtAuthController> log)
        {
            _ctx = ctx;
            _userManager = userManager;
            _log = log;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            _log.LogDebug("Login attempt by user {0}...", model.UserName);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return BadRequest("User not found");

            bool isValidLogin = await _userManager.CheckPasswordAsync(user, model.Password);
            if (isValidLogin)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var accessTokenString = GenerateAccessTokenString(claims);
                var refreshTokenString = await NewRefreshToken(model.ClientId, user.UserName, accessTokenString);

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
            _log.LogDebug("Registration attempt by user {0}...", model.UserName);

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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
            if (principal == null)
                return BadRequest("Principal for token not found");

            _log.LogDebug("Refreshing token for user {0}...", principal.Identity.Name);

            // retrieve the refresh token from a data store
            var savedRefreshToken = await _ctx.RefreshTokens.FindAsync(model.RefreshToken);
            if (savedRefreshToken == null || 
                savedRefreshToken.ClientId != model.ClientId ||
                savedRefreshToken.ProtectedTicket != model.AccessToken)
                return BadRequest("Invalid refresh token");

            _ctx.RefreshTokens.Remove(savedRefreshToken);
            await _ctx.SaveChangesAsync();

            var accessTokenString = GenerateAccessTokenString(principal.Claims);
            var refreshTokenString = await NewRefreshToken(model.ClientId, principal.Identity.Name, accessTokenString);

            return new ObjectResult(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
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

            var existingToken = _ctx.RefreshTokens.SingleOrDefault(r => r.Subject == newRefreshToken.Subject && r.ClientId == newRefreshToken.ClientId);

            if (existingToken != null)
                _ctx.RefreshTokens.Remove(existingToken);

            _ctx.RefreshTokens.Add(newRefreshToken);

            await _ctx.SaveChangesAsync();

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
