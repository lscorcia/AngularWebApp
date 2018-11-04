using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<RefreshToken>> List()
        {
            log.LogInformation("Retrieving refresh tokens...");
            return authController.GetRefreshTokens().ToList();
        }

        [HttpDelete("{*id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string id)
        {
            log.LogInformation("Deleting refresh token '{0}'...", id);
            await authController.DeleteRefreshToken(id);

            return Ok();
        }
    }
}