using aspnetcore.ntier.DTO.DTOs;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IUserService
{
    Task<List<UserDTO>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<UserDTO> GetUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserDTO> AddUserAsync(UserToAddDTO userToAddDTO);
    Task<UserDTO> UpdateUserAsync(UserToUpdateDTO userToUpdateDTO);
    Task DeleteUserAsync(int userId);
}
