using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.UserName == "johndoe" && user.Password == "def@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "AngularWebApp.Web",
                    audience: "AngularWebApp.Web.Client",
                    claims: new List<Claim>() { new Claim(ClaimTypes.Name, user.UserName) },
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("windowslogin")]
        [Authorize(AuthenticationSchemes = "Windows")]
        public IActionResult WindowsLogin()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "AngularWebApp.Web",
                audience: "AngularWebApp.Web.Client",
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, User.Identity.Name),
                    new Claim(ClaimTypes.Email, User.Identity.Name),
                },
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new { UserName = User.Identity.Name, Token = tokenString });
        }

        // GET api/values
        [HttpPost, Route("register")]
        public IActionResult Register([FromBody]RegisterModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
    }
}
