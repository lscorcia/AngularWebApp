using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.DI;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Services
{
    public class UserRolesService: IApplicationService
    {
        private readonly ILogger<UserRolesService> log;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRolesService(ILogger<UserRolesService> _log, UserManager<ApplicationUser> _userManager)
        {
            log = _log;
            userManager = _userManager;
        }

        public IEnumerable<GetUserRolesOutputDto> GetAll()
        {
            return userManager.Users.Include(t => t.UserRoles).ThenInclude(ur => ur.Role)
                .SelectMany(t => t.UserRoles)
                .Select(t => new GetUserRolesOutputDto() { UserName = t.User.UserName, Role = t.Role.Name })
                .OrderBy(t => t.Role).ThenBy(t => t.UserName);
        }

        public async Task Add(AddUserToRoleInputDto model)
        {
            log.LogInformation("Adding user {0} to role {1}", model.UserName, model.Role);

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
                throw new Exception(String.Format("User '{0}' not found!", model.UserName));

            await userManager.AddToRoleAsync(user, model.Role);
        }

        public async Task Delete(DeleteUserFromRoleInputDto model)
        {
            log.LogInformation("Deleting user {0} from role {1}", model.UserName, model.RoleName);

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
                await userManager.RemoveFromRoleAsync(user, model.RoleName);
        }
    }
}
