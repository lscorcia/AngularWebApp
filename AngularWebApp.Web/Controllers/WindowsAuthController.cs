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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Web.Controllers
{
    [Route("sso/auth")]
    public class WindowsAuthController : Controller
    {
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

        private readonly IConfiguration configuration;

        public WindowsAuthController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpPost, Route("windowslogin")]
        [Authorize(AuthenticationSchemes = "Windows")]
        public async Task<IActionResult> WindowsLogin([FromBody]WindowsLoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Identity.Name),
                new Claim(ClaimTypes.Email, User.Identity.Name),
            };

            var accessTokenString = GenerateAccessTokenString(claims);
            var refreshTokenString = await NewRefreshToken(model.ClientId, User.Identity.Name, accessTokenString);

            return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
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
            using (AuthRepository rpAuth = new AuthRepository(configuration))
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
        #endregion
    }
}
