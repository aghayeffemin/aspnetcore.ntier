using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult HelloWorld() 
    {
        return Ok("Hello World");
    }
}
