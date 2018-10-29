using System.Threading.Tasks;
using AngularWebApp.Web.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RefreshTokensController : ControllerBase
    {
        private readonly AuthDbContext _ctx;
        private readonly ILogger<RefreshTokensController> _log;

        public RefreshTokensController(AuthDbContext ctx, ILogger<RefreshTokensController> log)
        {
            _ctx = ctx;
            _log = log;
        }

        [HttpGet]
        public IActionResult List()
        {
            _log.LogInformation("Retrieving refresh tokens...");

            using (AuthRepository rpAuth = new AuthRepository(_ctx))
            {
                return Ok(rpAuth.GetAllRefreshTokens());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            _log.LogInformation("Deleting refresh token ID {0}...", id);

            using (AuthRepository rpAuth = new AuthRepository(_ctx))
            {
                var result = await rpAuth.RemoveRefreshToken(id);
                if (!result)
                    return BadRequest("Token Id does not exist");

                return Ok();
            }
        }
    }
}