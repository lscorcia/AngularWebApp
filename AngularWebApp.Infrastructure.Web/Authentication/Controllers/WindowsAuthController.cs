using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.ActiveDirectory.Models;
using AngularWebApp.Infrastructure.ActiveDirectory.Services;
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
        private readonly ActiveDirectoryService activeDirectoryController;

        public WindowsAuthController(AuthController _authController, ILogger<WindowsAuthController> _log,
            ActiveDirectoryService _activeDirectoryController,
            UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            authController = _authController;
            activeDirectoryController = _activeDirectoryController;
            log = _log;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Windows")]
        public async Task<IActionResult> Login(WindowsLoginModel model)
        {
            string userName = User.Identity.Name;
            log.LogInformation("Windows login for user {0}...", userName);

            var dtoGetUserInfo = new GetUserInfoByAccountNameInputDto();
            dtoGetUserInfo.SamAccountName = userName;
            var userInfo = activeDirectoryController.GetUserInfoByAccountName(dtoGetUserInfo);

            var user = await userManager.FindByLoginAsync("AD", userName);
            if (user == null)
            {
                var newUser = new IdentityUser { UserName = userName, Email = userInfo.Email };
                var createResult = await userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    ModelState.AddModelError("", String.Join(", ", createResult.Errors));
                    return BadRequest(ModelState);
                }

                var userLoginInfo = new UserLoginInfo("AD", userName, userName);
                var addLoginResult = await userManager.AddLoginAsync(newUser, userLoginInfo);
                if (!addLoginResult.Succeeded)
                {
                    ModelState.AddModelError("", String.Join(", ", addLoginResult.Errors));
                    return BadRequest(ModelState);
                }
            }

            var claims = await GetUserClaims(user);
            var accessTokenString = authController.GenerateAccessTokenString(claims);
            var refreshTokenString = await authController.NewRefreshToken(model.ClientId, userName, accessTokenString);

            return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
        }

        #region Private Helpers
        private async Task<List<Claim>> GetUserClaims(IdentityUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            if (userClaims != null)
                claims.AddRange(userClaims);

            // Add roles
            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles != null)
            { 
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    // Add role claims
                    var role = await roleManager.FindByNameAsync(userRole);
                    if (role != null)
                    {
                        var roleClaims = await roleManager.GetClaimsAsync(role);
                        if (roleClaims != null)
                        { 
                            foreach (Claim roleClaim in roleClaims)
                                claims.Add(roleClaim);
                        }
                    }
                }
            }

            return claims;
        }
        #endregion
    }
}
