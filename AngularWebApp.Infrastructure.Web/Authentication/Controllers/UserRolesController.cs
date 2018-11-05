using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly ILogger<UserRolesController> log;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRolesController(ILogger<UserRolesController> _log,
            UserManager<ApplicationUser> _userManager)
        {
            log = _log;
            userManager = _userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<GetUserRolesOutputDto>> List()
        {
            return userManager.Users.Include(t => t.UserRoles).ThenInclude(ur => ur.Role)
                .SelectMany(t => t.UserRoles)
                .Select(t => new GetUserRolesOutputDto() { UserName = t.User.UserName, Role = t.Role.Name })
                .OrderBy(t => t.Role).ThenBy(t => t.UserName)
                .ToList();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Add(AddUserToRoleInputDto model)
        {
            log.LogInformation("Adding user {0} to role {1}", model.UserName, model.Role);

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
                await userManager.AddToRoleAsync(user, model.Role);

            return Ok();
        }

        [HttpDelete("{roleName}/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string roleName, string userName)
        {
            log.LogInformation("Deleting user {0} from role {1}", userName, roleName);

            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
                await userManager.RemoveFromRoleAsync(user, roleName);

            return Ok();
        }
    }
}
