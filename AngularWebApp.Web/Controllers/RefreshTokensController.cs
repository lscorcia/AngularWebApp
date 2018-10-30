using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Controllers;
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
            return await authController.DeleteRefreshToken(id);
        }
    }
}