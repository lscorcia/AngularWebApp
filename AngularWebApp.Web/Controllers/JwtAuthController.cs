using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularWebApp.Web.Authentication;
using AngularWebApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtAuthController : ControllerBase
    {
        private readonly AuthController authController;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<JwtAuthController> log;

        public JwtAuthController(AuthController _authController, UserManager<IdentityUser> _userManager, ILogger<JwtAuthController> _log)
        {
            authController = _authController;
            userManager = _userManager;
            log = _log;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            log.LogDebug("Login attempt by user {0}...", model.UserName);

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return BadRequest("User not found");

            bool isValidLogin = await userManager.CheckPasswordAsync(user, model.Password);
            if (isValidLogin)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

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

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
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
                return BadRequest("Principal for token not found");

            log.LogDebug("Refreshing token for user {0}...", principal.Identity.Name);

            // retrieve the refresh token from a data store
            await authController.RemoveExistingRefreshToken(model.ClientId, model.AccessToken, model.RefreshToken);
            
            var accessTokenString = authController.GenerateAccessTokenString(principal.Claims);
            var refreshTokenString = await authController.NewRefreshToken(model.ClientId, principal.Identity.Name, accessTokenString);

            return new ObjectResult(new { AccessToken = accessTokenString, RefreshToken = refreshTokenString });
        }
    }
}
