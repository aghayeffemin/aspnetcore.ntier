using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers.V2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserToLoginDto userToLoginDto)
    {
        try
        {
            var user = await authService.LoginAsync(userToLoginDto);

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
    public async Task<IActionResult> Register(UserToRegisterDto userToRegisterDto)
    {
        try
        {
            return Ok(await authService.RegisterAsync(userToRegisterDto));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPost("refresh")]
    public IActionResult RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            return Ok(authService.RefreshToken(refreshTokenDto));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }
}
