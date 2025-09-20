using aspnetcore.ntier.DTO.Dtos;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IUserService
{
    Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<UserDto> GetUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserDto> AddUserAsync(UserToAddDto userToAddDto);
    Task<UserDto> UpdateUserAsync(UserToUpdateDto userToUpdateDto);
    Task DeleteUserAsync(int userId);
}
