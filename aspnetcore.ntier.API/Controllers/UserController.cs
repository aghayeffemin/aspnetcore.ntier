using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("getusers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return Ok(await userService.GetUsersAsync());
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpGet("getuser")]
    public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await userService.GetUserAsync(userId, cancellationToken));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPost("adduser")]
    public async Task<IActionResult> AddUser(UserToAddDto userToAddDto)
    {
        try
        {
            return Ok(await userService.AddUserAsync(userToAddDto));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPut("updateuser")]
    public async Task<IActionResult> UpdateUser(UserToUpdateDto userToUpdateDto)
    {
        try
        {
            return Ok(await userService.UpdateUserAsync(userToUpdateDto));
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpDelete("deleteuser")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            await userService.DeleteUserAsync(userId);
            return Ok();
        }
        catch (UserNotFoundException)
        {
            return NotFound("User not found");
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }
}
