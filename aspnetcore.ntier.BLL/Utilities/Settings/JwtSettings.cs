namespace aspnetcore.ntier.BLL.Utilities.Settings;

public class JwtSettings
{
    public required string AccessTokenSecret { get; set; }
    public required string RefreshTokenSecret { get; set; }
    public double AccessTokenExpirationMinutes { get; set; }
    public double RefreshTokenExpirationMinutes { get; set; }
}
