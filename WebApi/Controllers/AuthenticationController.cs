using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Auth;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtAuthentication authentication;
        private readonly UserManager<Person> manager;

        public AuthenticationController(JwtAuthentication authentication, UserManager<Person> manager)
        {
            this.authentication = authentication;
            this.manager = manager;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticationDto auth)
        {
            var token = await authentication.AuthenticateAsync(auth.Username, auth.Password);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto auth)
        {
            try
            {
                var newUser = new Person { UserName = auth.Username, FirstName = auth.FirstName, LastName = auth.LastName, Email = auth.Email };

                var result = await manager.CreateAsync(newUser, auth.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                var res = await manager.AddToRoleAsync(newUser, auth.Role);

                if (res.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(res.Errors);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
