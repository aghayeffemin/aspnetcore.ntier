namespace aspnetcore.ntier.DTO.Dtos;

public class UserToUpdateDto
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
}
