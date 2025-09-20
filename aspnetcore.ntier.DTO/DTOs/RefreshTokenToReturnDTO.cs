namespace aspnetcore.ntier.DTO.Dtos;

public class RefreshTokenToReturnDto
{
    public required string Username { get; set; }
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
