using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers;

[Authorize]
[Route("api/[controller]")]
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
            return Ok(await _userService.GetUsers());
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpGet("getuser")]
    public async Task<IActionResult> GetUser(int userId)
    {
        try
        {
            return Ok(await _userService.GetUser(userId));
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
            return Ok(await _userService.AddUser(userToAddDTO));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPut("updateuser")]
    public async Task<IActionResult> UpdateUser(UserDTO userToUpdateDTO)
    {
        try
        {
            return Ok(await _userService.UpdateUser(userToUpdateDTO));
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
            await _userService.DeleteUser(userId);
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
