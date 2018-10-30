using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("sso/[controller]/[action]")]
    [ApiController]
    public class WindowsAuthController : ControllerBase
    {
        private readonly AuthController authController;
        private readonly ILogger<WindowsAuthController> log;

        public WindowsAuthController(AuthController _authController, ILogger<WindowsAuthController> _log)
        {
            authController = _authController;
            log = _log;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Windows")]
        public async Task<IActionResult> Login(WindowsLoginModel model)
        {
            log.LogInformation("Windows login for user {0}...", User.Identity.Name);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, User.Identity.Name),
                new Claim(ClaimTypes.Email, User.Identity.Name),
            };

            var accessTokenString = authController.GenerateAccessTokenString(claims);
            var refreshTokenString = await authController.NewRefreshToken(model.ClientId, User.Identity.Name, accessTokenString);

            return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
        }
    }
}
