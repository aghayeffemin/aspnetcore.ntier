namespace aspnetcore.ntier.DTO.Dtos;

public class UserToRegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
}
