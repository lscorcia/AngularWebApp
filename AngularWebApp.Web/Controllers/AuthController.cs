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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

        // GET api/values
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.UserName == "johndoe" && user.Password == "def@123")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, User.Identity.Name),
                    new Claim(ClaimTypes.Email, User.Identity.Name),
                };
                var tokenString = GenerateToken(claims);

                return Ok(new { Token = tokenString, RefreshToken = await NewRefreshToken(User.Identity.Name, claims) });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("windowslogin")]
        [Authorize(AuthenticationSchemes = "Windows")]
        public async Task<IActionResult> WindowsLogin()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Identity.Name),
                new Claim(ClaimTypes.Email, User.Identity.Name),
            };
            var tokenString = GenerateToken(claims);

            return Ok(new { Token = tokenString, RefreshToken = await NewRefreshToken(User.Identity.Name, claims) });
        }

        // GET api/values
        [HttpPost, Route("register")]
        public IActionResult Register([FromBody]RegisterModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(string token, string refreshToken)
        {
            using (AuthRepository rpAuth = new AuthRepository())
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var username = principal.Identity.Name;

                var savedRefreshToken = await rpAuth.FindRefreshToken(username); ; //retrieve the refresh token from a data store
                if (savedRefreshToken.ProtectedTicket != refreshToken)
                    throw new SecurityTokenException("Invalid refresh token");

                await rpAuth.RemoveRefreshToken(savedRefreshToken);

                var newJwtToken = await NewRefreshToken(principal.Identity.Name, principal.Claims);

                return new ObjectResult(new
                {
                    token = newJwtToken,
                    refreshToken = newJwtToken
                });
            }
        }

        private async Task<string> NewRefreshToken(string username, IEnumerable<Claim> claims)
        {
            using (AuthRepository rpAuth = new AuthRepository())
            {
                var newJwtToken = GenerateToken(claims);
                var newRefreshToken = GenerateRefreshToken();

                var newNewRefreshToken = new RefreshToken();
                newNewRefreshToken.ProtectedTicket = newRefreshToken;
                newNewRefreshToken.IssuedUtc = DateTime.UtcNow;
                newNewRefreshToken.ExpiresUtc = DateTime.UtcNow.AddDays(1);
                newNewRefreshToken.Subject = username;
                await rpAuth.AddRefreshToken(newNewRefreshToken);

                return newJwtToken;
            }
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(issuer: "AngularWebApp.Web",
                audience: "AngularWebApp.Web.Client",
                claims: claims, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //... 
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt); //the method is called WriteToken but returns a string
        }

        public string GenerateRefreshToken()
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
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the server key used to sign the JWT token is here, use more than 16 chars")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
