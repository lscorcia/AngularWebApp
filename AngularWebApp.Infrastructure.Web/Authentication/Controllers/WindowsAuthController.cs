using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public WindowsAuthController(AuthController _authController, ILogger<WindowsAuthController> _log,
            UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            authController = _authController;
            log = _log;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Windows")]
        public async Task<IActionResult> Login(WindowsLoginModel model)
        {
            log.LogInformation("Windows login for user {0}...", User.Identity.Name);

            var claims = await GetUserClaims(User.Identity.Name);
            var accessTokenString = authController.GenerateAccessTokenString(claims);
            var refreshTokenString = await authController.NewRefreshToken(model.ClientId, User.Identity.Name, accessTokenString);

            return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
        }

        #region Private Helpers
        private async Task<List<Claim>> GetUserClaims(string userName)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userName),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userName)
            };
            /*
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Add roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                // Add role claims
                var role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                        claims.Add(roleClaim);
                }
            }
            */
            return claims;
        }
        #endregion
    }
}
