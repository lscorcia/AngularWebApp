using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Web.Authentication.Models;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using AngularWebApp.Infrastructure.Web.Authentication.Services;
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
        private readonly RolesService rolesService;

        public RolesController(ILogger<WindowsAuthController> _log,
            RolesService _rolesService,
            RoleManager<ApplicationRole> _roleManager)
        {
            log = _log;
            rolesService = _rolesService;
            roleManager = _roleManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<GetRoleOutputDto>> List()
        {
            return rolesService.GetAll().ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GetRoleOutputDto> Get(string id)
        {
            var item = rolesService.GetOne(id);
            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<string>> Add(AddRoleInputDto model)
        {
            var roleId = await rolesService.Add(model);

            return CreatedAtAction(nameof(Get), new { id = roleId }, new { id = roleId });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Edit(EditRoleInputDto model)
        {
            await rolesService.Edit(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string id)
        {
            await rolesService.Delete(id);

            return Ok();
        }
    }
}
