using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using AngularWebApp.Infrastructure.Web.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrators")]
    public class RefreshTokensController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly ILogger<RefreshTokensController> log;

        public RefreshTokensController(AuthService _authService, ILogger<RefreshTokensController> _log)
        {
            authService = _authService;
            log = _log;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<RefreshToken>> List()
        {
            return authService.GetRefreshTokens().ToList();
        }

        [HttpDelete("{*id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string id)
        {
            await authService.DeleteRefreshToken(id);
            return Ok();
        }
    }
}