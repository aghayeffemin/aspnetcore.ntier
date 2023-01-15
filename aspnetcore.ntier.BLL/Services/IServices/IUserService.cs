using aspnetcore.ntier.DTO.DTOs;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IUserService
{
    Task<List<UserDTO>> GetUsers();
    Task<UserDTO> GetUser(int userId);
    Task<UserDTO> AddUser(UserToAddDTO userToAddDTO);
    Task<UserDTO> UpdateUser(UserDTO userToUpdateDTO);
    Task DeleteUser(int userId);
}
