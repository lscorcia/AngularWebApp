using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.DI;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Infrastructure.Web.Authentication.Services
{
    public class AuthService : IApplicationService
    {
        private readonly ILogger<AuthService> log;
        private readonly IConfiguration config;
        private readonly AuthDbContext authDb;

        public AuthService(IServiceProvider _sp, IConfiguration _config, ILogger<AuthService> _log)
        {
            log = _log;
            config = _config;
           
            // Objects obtained through DI shouldn't be disposed
            // We need to locate the service explicitly because the class is not public and can't be injected
            authDb = _sp.GetService<AuthDbContext>();
        }

        public async Task<string> NewRefreshToken(string clientId, string username, string accessToken)
        {
            var refreshTokenExpiration = config.GetValue<int>("RefreshToken:Expiration");

            var newRefreshTokenString = GenerateRefreshTokenString();

            var newRefreshToken = new RefreshToken();
            newRefreshToken.Id = newRefreshTokenString;
            newRefreshToken.ClientId = clientId;
            newRefreshToken.Subject = username;
            newRefreshToken.ProtectedTicket = accessToken;
            newRefreshToken.IssuedUtc = DateTime.UtcNow;
            newRefreshToken.ExpiresUtc = DateTime.UtcNow.AddSeconds(refreshTokenExpiration);

            var existingToken = authDb.RefreshTokens.SingleOrDefault(r => r.Subject == newRefreshToken.Subject && r.ClientId == newRefreshToken.ClientId);
                if (existingToken != null)
                    authDb.RefreshTokens.Remove(existingToken);

            authDb.RefreshTokens.Add(newRefreshToken);

            await authDb.SaveChangesAsync();

            return newRefreshTokenString;
        }

        public string GenerateAccessTokenString(IEnumerable<Claim> claims)
        {
            var tokenIssuerString = config.GetValue<string>("JwtToken:Issuer");
            var tokenAudienceString = config.GetValue<string>("JwtToken:Audience");
            var tokenExpiration = config.GetValue<int>("JwtToken:Expiration");

            var jwt = new JwtSecurityToken(issuer: tokenIssuerString,
                audience: tokenAudienceString,
                claims: claims, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //... 
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(tokenExpiration),
                signingCredentials: new SigningCredentials(GetTokenSigningKey(), SecurityAlgorithms.HmacSha256)
            );

            // The method is called WriteToken but returns a string
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenIssuerString = config.GetValue<string>("JwtToken:Issuer");
            var tokenAudienceString = config.GetValue<string>("JwtToken:Audience");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // Here we are saying that we don't care about the token's expiration date

                ValidIssuer = tokenIssuerString,
                ValidAudience = tokenAudienceString,
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
            var savedRefreshToken = await authDb.RefreshTokens.FindAsync(refreshToken);
            if (savedRefreshToken == null ||
                savedRefreshToken.ClientId != clientId ||
                savedRefreshToken.ProtectedTicket != accessToken)
                throw new Exception("Invalid refresh token");

            authDb.RefreshTokens.Remove(savedRefreshToken);
            await authDb.SaveChangesAsync();
        }

        public IEnumerable<RefreshToken> GetRefreshTokens()
        {
            return authDb.RefreshTokens;
        }

        public async Task DeleteRefreshToken(string refreshTokenId)
        {
            log.LogInformation("Deleting refresh token ID {0}...", refreshTokenId);

            var refreshToken = await authDb.RefreshTokens.FindAsync(refreshTokenId);
            if (refreshToken != null)
            { 
                authDb.RefreshTokens.Remove(refreshToken);
                await authDb.SaveChangesAsync();
            }
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
            var tokenSigningKeyString = config.GetValue<string>("JwtToken:SigningKey");
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSigningKeyString));
        }
        #endregion
    }
}
