using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getusers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return Ok(await _userService.GetUsersAsync());
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
            return Ok(await _userService.GetUserAsync(userId, cancellationToken));
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
    public async Task<IActionResult> AddUser(UserToAddDTO userToAddDTO)
    {
        try
        {
            return Ok(await _userService.AddUserAsync(userToAddDTO));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPut("updateuser")]
    public async Task<IActionResult> UpdateUser(UserToUpdateDTO userToUpdateDTO)
    {
        try
        {
            return Ok(await _userService.UpdateUserAsync(userToUpdateDTO));
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
            await _userService.DeleteUserAsync(userId);
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
