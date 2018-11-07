using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using AngularWebApp.Infrastructure.Web.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Infrastructure.Web.Authentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrators")]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<WindowsAuthController> log;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RolesController(ILogger<WindowsAuthController> _log,
            RoleManager<ApplicationRole> _roleManager)
        {
            log = _log;
            roleManager = _roleManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<GetRoleOutputDto>> List()
        {
            return roleManager.Roles
                .Select(t => new GetRoleOutputDto() { Id = t.Id, Name = t.Name })
                .OrderBy(t => t.Name)
                .ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GetRoleOutputDto> Get(string id)
        {
            var entity = roleManager.Roles.FirstOrDefault(t => t.Id == id);
            if (entity == null)
                return NotFound();

            return new GetRoleOutputDto() { Id = entity.Id, Name = entity.Name };
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<string>> Add(AddRoleInputDto model)
        {
            log.LogInformation("Adding role {0}", model.Name);

            bool roleExists = await roleManager.RoleExistsAsync(model.Name);
            if (roleExists)
                return Conflict(new ErrorResponse(String.Format("Role '{0}' already exists!", model.Name)));

            var role = new ApplicationRole(model.Name);
            await roleManager.CreateAsync(role);

            return CreatedAtAction(nameof(Get), new { id = role.Id }, new { id = role.Id });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Edit(EditRoleInputDto model)
        {
            log.LogInformation("Editing role {0} - new name = {1}", model.Id, model.Name);

            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return NotFound(new ErrorResponse(String.Format("Role ID '{0}' not found!", model.Id)));

            role.Name = model.Name;
            await roleManager.UpdateAsync(role);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string id)
        {
            log.LogInformation("Deleting role {0}", id);

            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
                await roleManager.DeleteAsync(role);

            return Ok();
        }
    }
}
