﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using AngularWebApp.Infrastructure.Web.ErrorHandling;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtAuthController : ControllerBase
    {
        private readonly AuthController authController;
        private readonly ILogger<JwtAuthController> log;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public JwtAuthController(AuthController _authController, ILogger<JwtAuthController> _log,
            UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager)
        {
            authController = _authController;
            log = _log;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            log.LogDebug("Login attempt by user {0}...", model.UserName);

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return BadRequest(new ErrorResponse("User not found"));

            bool isValidLogin = await userManager.CheckPasswordAsync(user, model.Password);
            if (isValidLogin)
            {
                var claims = await GetUserClaims(user);
                var accessTokenString = authController.GenerateAccessTokenString(claims);
                var refreshTokenString = await authController.NewRefreshToken(model.ClientId, user.UserName, accessTokenString);

                return Ok(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            log.LogDebug("Registration attempt by user {0}...", model.UserName);

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(RefreshTokenModel model)
        {
            var principal = authController.GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal == null)
                return BadRequest(new ErrorResponse("Principal for token not found"));

            log.LogDebug("Refreshing token for user {0}...", principal.Identity.Name);

            // retrieve the refresh token from a data store
            await authController.RemoveExistingRefreshToken(model.ClientId, model.AccessToken, model.RefreshToken);
            
            var accessTokenString = authController.GenerateAccessTokenString(principal.Claims);
            var refreshTokenString = await authController.NewRefreshToken(model.ClientId, principal.Identity.Name, accessTokenString);

            return new ObjectResult(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
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

            return claims;
        }
        #endregion
    }
}
