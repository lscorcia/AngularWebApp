using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Web.Authentication.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Web.Authentication.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> log;
        private readonly IConfiguration config;
        private readonly AuthDbContext ctx;

        public AuthController(AuthDbContext _ctx, IConfiguration _config, ILogger<AuthController> _log)
        {
            log = _log;
            ctx = _ctx;
            config = _config;
        }

        public async Task<string> NewRefreshToken(string clientId, string username, string accessToken)
        {
            var newRefreshTokenString = GenerateRefreshTokenString();

            var newRefreshToken = new RefreshToken();
            newRefreshToken.Id = newRefreshTokenString;
            newRefreshToken.ClientId = clientId;
            newRefreshToken.Subject = username;
            newRefreshToken.ProtectedTicket = accessToken;
            newRefreshToken.IssuedUtc = DateTime.UtcNow;
            newRefreshToken.ExpiresUtc = DateTime.UtcNow.AddDays(1);

            var existingToken = ctx.RefreshTokens.SingleOrDefault(r => r.Subject == newRefreshToken.Subject && r.ClientId == newRefreshToken.ClientId);

            if (existingToken != null)
                ctx.RefreshTokens.Remove(existingToken);

            ctx.RefreshTokens.Add(newRefreshToken);

            await ctx.SaveChangesAsync();

            return newRefreshTokenString;
        }

        public string GenerateAccessTokenString(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(issuer: "AngularWebApp.Web",
                audience: "AngularWebApp.Web.Client",
                claims: claims, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //... 
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: new SigningCredentials(GetTokenSigningKey(), SecurityAlgorithms.HmacSha256)
            );

            // The method is called WriteToken but returns a string
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // Here we are saying that we don't care about the token's expiration date

                ValidIssuer = "AngularWebApp.Web",
                ValidAudience = "AngularWebApp.Web.Client",
                IssuerSigningKey = GetTokenSigningKey()
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task RemoveExistingRefreshToken(string clientId, string accessToken, string refreshToken)
        {
            var savedRefreshToken = await ctx.RefreshTokens.FindAsync(refreshToken);
            if (savedRefreshToken == null ||
                savedRefreshToken.ClientId != clientId ||
                savedRefreshToken.ProtectedTicket != accessToken)
                throw new Exception("Invalid refresh token");

            ctx.RefreshTokens.Remove(savedRefreshToken);
            await ctx.SaveChangesAsync();
        }

        #region Private Helpers
        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private SymmetricSecurityKey GetTokenSigningKey()
        {
            var tokenSigningKeyString = config.GetValue<string>("JwtTokenSigningKey");
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSigningKeyString));
        }
        #endregion
    }
}
