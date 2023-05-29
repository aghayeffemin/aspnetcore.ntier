using aspnetcore.ntier.DTO.DTOs;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IAuthService
{
    Task<UserToReturnDTO> LoginAsync(UserToLoginDTO userToLoginDTO);
    Task<UserToReturnDTO> RegisterAsync(UserToRegisterDTO userToRegisterDTO);
}
