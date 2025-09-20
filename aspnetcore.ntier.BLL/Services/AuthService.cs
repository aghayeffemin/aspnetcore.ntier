using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.BLL.Utilities.Settings;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.Dtos;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace aspnetcore.ntier.BLL.Services;

public class AuthService(
    IUserRepository userRepository,
    IMapper mapper,
    IOptions<JwtSettings> jwtSettings) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<UserToReturnDto> LoginAsync(UserToLoginDto userToLoginDto)
    {
        var user = await userRepository.GetAsync(
                    u => u.Username == userToLoginDto.Username.ToLower() && u.Password == userToLoginDto.Password);

        if (user == null)
            throw new UserNotFoundException();

        var userToReturn = mapper.Map<UserToReturnDto>(user);
        userToReturn.Token = GenerateToken(user.UserId, user.Username);
        userToReturn.RefreshToken = GenerateToken(user.UserId, user.Username, true);

        return userToReturn;
    }

    public async Task<UserToReturnDto> RegisterAsync(UserToRegisterDto userToRegisterDto)
    {
        userToRegisterDto.Username = userToRegisterDto.Username.ToLower();

        var addedUser = await userRepository.AddAsync(mapper.Map<User>(userToRegisterDto));

        var userToReturn = mapper.Map<UserToReturnDto>(addedUser);
        userToReturn.Token = GenerateToken(addedUser.UserId, addedUser.Username);
        userToReturn.RefreshToken = GenerateToken(addedUser.UserId, addedUser.Username, true);

        return userToReturn;
    }

    public RefreshTokenToReturnDto RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var claimsPrincipal = GetClaimsPrincipal(refreshTokenDto.RefreshToken);
        if (claimsPrincipal is null)
            throw new BadRequestException();

        var username = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Name)?.FirstOrDefault()?.Value ?? string.Empty;
        var userId = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? string.Empty;
        if (userId == string.Empty || username == string.Empty)
            throw new BadRequestException();

        var refreshToken = GenerateToken(int.Parse(userId), username, true);
        var accessToken = GenerateToken(int.Parse(userId), username);

        return new RefreshTokenToReturnDto
        {
            Username = username,
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string GenerateToken(int userId, string username, bool isRefresh = false)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var tokenSecret = isRefresh ? _jwtSettings.RefreshTokenSecret : _jwtSettings.AccessTokenSecret;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var expirationInMinutes = isRefresh ? _jwtSettings.AccessTokenExpirationMinutes : _jwtSettings.RefreshTokenExpirationMinutes;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetClaimsPrincipal(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret)),
            ClockSkew = TimeSpan.Zero
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters, out var _);
        }
        catch (Exception)
        {
            throw new BadRequestException();
        }
    }
}