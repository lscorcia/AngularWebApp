using System.Threading.Tasks;
using AngularWebApp.Web.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RefreshTokensController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public RefreshTokensController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet]
        public IActionResult List()
        {
            using (AuthRepository rpAuth = new AuthRepository(configuration))
            {
                return Ok(rpAuth.GetAllRefreshTokens());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            using (AuthRepository rpAuth = new AuthRepository(configuration))
            {
                var result = await rpAuth.RemoveRefreshToken(id);
                if (!result)
                    return BadRequest("Token Id does not exist");

                return Ok();
            }
        }
    }
}