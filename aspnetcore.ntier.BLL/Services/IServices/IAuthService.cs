using aspnetcore.ntier.DTO.DTOs;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IAuthService
{
    Task<UserDTO> Login(UserToLoginDTO userToLoginDTO);
    Task<UserDTO> Register(UserToRegisterDTO userToRegisterDTO);
}
