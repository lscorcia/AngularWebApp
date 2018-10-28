using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularWebApp.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Web.Authentication
{
    public class AuthRepository : IDisposable
    {
        private readonly AuthDbContext _ctx;

        public AuthRepository(AuthDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Dispose()
        {
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = _ctx.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);
            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }
    }
}