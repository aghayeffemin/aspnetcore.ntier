using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.ntier.API.Controllers.V2;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getusers")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _userService.GetUsersAsync(cancellationToken));
        }
        catch (Exception)
        {
            return BadRequest("Something went wrong");
        }
    }
}
