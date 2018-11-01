using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RefreshTokensController : ControllerBase
    {
        private readonly AuthController authController;
        private readonly ILogger<RefreshTokensController> log;

        public RefreshTokensController(AuthController _authController, ILogger<RefreshTokensController> _log)
        {
            authController = _authController;
            log = _log;
        }

        [HttpGet]
        public IActionResult List()
        {
            log.LogInformation("Retrieving refresh tokens...");
            return Ok(authController.GetRefreshTokens());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            log.LogInformation("Deleting refresh token '{0}'...", id);
            return await authController.DeleteRefreshToken(id);
        }
    }
}