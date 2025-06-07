using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/auth")]

    public class AuthController : ControllerBase
    {
        private readonly AutheService _autheService;
        public AuthController(AutheService autheService)
        {
            _autheService = autheService;
        }

        [HttpPost("sigin")]
        public async Task<ActionResult> Sigin([FromBody] SiginDTO user)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Erreur sur le champ {entry.Key} : {error.ErrorMessage}");
                    }
                }
                return BadRequest(ModelState);
            }

            await _autheService.Sigin(user);
            return Ok();
        }


        [HttpPost("role")]
        public async Task<ActionResult> RoleAsync(RoleDTO role)
        {
            await _autheService.RoleAsync(role);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            var token = await _autheService.Login(login);
            return Ok(token);
        }
    }
}
