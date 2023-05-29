using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers.V2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserToLoginDTO userToLoginDTO)
    {
        try
        {
            var user = await _authService.LoginAsync(userToLoginDTO);

            return Ok(user);
        }
        catch (UserNotFoundException)
        {
            return Unauthorized();
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserToRegisterDTO userToRegisterDTO)
    {
        try
        {
            return Ok(await _authService.RegisterAsync(userToRegisterDTO));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }
}
