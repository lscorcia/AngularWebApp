using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.ActiveDirectory.Models;
using AngularWebApp.Infrastructure.ActiveDirectory.Services;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using AngularWebApp.Infrastructure.Web.Authentication.Services;
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
        private readonly AuthService authService;
        private readonly ILogger<WindowsAuthController> log;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ActiveDirectoryService activeDirectoryController;

        public WindowsAuthController(AuthService _authService, ILogger<WindowsAuthController> _log,
            ActiveDirectoryService _activeDirectoryController,
            UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
        {
            authService = _authService;
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
                var newUser = new ApplicationUser { UserName = userName, Email = userInfo.Email };
                var createResult = await userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    ModelState.AddModelError("", String.Join(", ", createResult.Errors));
                    return BadRequest(ModelState);
                }

                var userLoginInfo = new UserLoginInfo("AD", userName, userInfo.DisplayName);
                var addLoginResult = await userManager.AddLoginAsync(newUser, userLoginInfo);
                if (!addLoginResult.Succeeded)
                {
                    ModelState.AddModelError("", String.Join(", ", addLoginResult.Errors));
                    return BadRequest(ModelState);
                }
            }

            var claims = await GetUserClaims(user);
            var accessTokenString = authService.GenerateAccessTokenString(claims);
            var refreshTokenString = await authService.NewRefreshToken(model.ClientId, userName, accessTokenString);

            return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
        }

        #region Private Helpers
        private async Task<List<Claim>> GetUserClaims(ApplicationUser user)
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
