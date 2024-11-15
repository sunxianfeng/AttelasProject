using Attelas.Models;
using Attelas.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Attelas.Controllers;

[ApiController]
[Route("/v1/login")]
public class LoginController : ControllerBase
{
    [HttpPost("")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (model.Username == "validUser" && model.Password == "validPassword")
        {
            var token = JwtHelper.CreateToken(model.Username);
            return Ok(new { token });
        }
 
        return Unauthorized();
    }
    
}

