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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<WindowsAuthController> log;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ILogger<WindowsAuthController> _log,
            RoleManager<IdentityRole> _roleManager)
        {
            log = _log;
            roleManager = _roleManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRoleInputDto model)
        {
            bool roleExists = await roleManager.RoleExistsAsync(model.Name);
            if (!roleExists)
            {
                var role = new IdentityRole(model.Name);
                await roleManager.CreateAsync(role);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleInputDto model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return BadRequest(String.Format("Role ID {0} not found", model.Id));

            role.Name = model.Name;

            await roleManager.UpdateAsync(role);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(EditRoleInputDto model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return Ok();

            await roleManager.DeleteAsync(role);

            return Ok();
        }
    }
}
