using System.Threading.Tasks;
using AngularWebApp.Web.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/RefreshTokens")]
    public class RefreshTokensController : Controller
    {
        private readonly IConfiguration configuration;

        public RefreshTokensController(IConfiguration config)
        {
            configuration = config;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            using (AuthRepository rpAuth = new AuthRepository(configuration))
            {
                return Ok(rpAuth.GetAllRefreshTokens());
            }
        }

        [AllowAnonymous]
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