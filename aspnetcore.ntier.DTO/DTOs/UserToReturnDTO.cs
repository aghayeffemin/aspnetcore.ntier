namespace aspnetcore.ntier.DTO.Dtos;

public class UserToReturnDto
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
