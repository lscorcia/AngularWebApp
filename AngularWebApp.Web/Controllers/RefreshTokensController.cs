using System.Threading.Tasks;
using AngularWebApp.Web.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RefreshTokensController : ControllerBase
    {
        private readonly AuthDbContext _ctx;

        public RefreshTokensController(AuthDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult List()
        {
            using (AuthRepository rpAuth = new AuthRepository(_ctx))
            {
                return Ok(rpAuth.GetAllRefreshTokens());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            using (AuthRepository rpAuth = new AuthRepository(_ctx))
            {
                var result = await rpAuth.RemoveRefreshToken(id);
                if (!result)
                    return BadRequest("Token Id does not exist");

                return Ok();
            }
        }
    }
}