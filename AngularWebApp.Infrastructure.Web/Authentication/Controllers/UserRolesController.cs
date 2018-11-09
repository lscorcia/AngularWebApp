using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrators")]
    public class UserRolesController : ControllerBase
    {
        private readonly UserRolesService userRolesServices;

        public UserRolesController(UserRolesService _userRolesServices)
        {
            userRolesServices = _userRolesServices;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<GetUserRolesOutputDto>> List()
        {
            return userRolesServices.GetAll().ToList();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Add(AddUserToRoleInputDto model)
        {
            await userRolesServices.Add(model);
            return Ok();
        }

        [HttpDelete("{roleName}/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string roleName, string userName)
        {
            DeleteUserFromRoleInputDto model = new DeleteUserFromRoleInputDto();
            model.RoleName = roleName;
            model.UserName = userName;

            await userRolesServices.Delete(model);
            return Ok();
        }
    }
}
