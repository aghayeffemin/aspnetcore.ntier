using aspnetcore.ntier.DTO.DTOs;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IAuthService
{
    Task<UserToReturnDTO> Login(UserToLoginDTO userToLoginDTO);
    Task<UserToReturnDTO> Register(UserToRegisterDTO userToRegisterDTO);
}
