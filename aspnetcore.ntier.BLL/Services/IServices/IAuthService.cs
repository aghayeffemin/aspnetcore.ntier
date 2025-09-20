using aspnetcore.ntier.DTO.Dtos;

namespace aspnetcore.ntier.BLL.Services.IServices;

public interface IAuthService
{
    Task<UserToReturnDto> LoginAsync(UserToLoginDto userToLoginDto);
    Task<UserToReturnDto> RegisterAsync(UserToRegisterDto userToRegisterDto);
    RefreshTokenToReturnDto RefreshToken(RefreshTokenDto refreshTokenDto);
}
