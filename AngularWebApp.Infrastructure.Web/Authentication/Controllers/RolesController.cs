using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    }
}
