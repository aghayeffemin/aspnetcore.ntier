using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.BLL.Utilities.Settings;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace aspnetcore.ntier.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,

        IMapper mapper,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<UserToReturnDTO> LoginAsync(UserToLoginDTO userToLoginDTO)
    {
        var user = await _userRepository.GetAsync(
            u => u.Username == userToLoginDTO.Username.ToLower() && u.Password == userToLoginDTO.Password);

        if (user == null)
            throw new UserNotFoundException();

        var userToReturn = _mapper.Map<UserToReturnDTO>(user);
        userToReturn.Token = GenerateToken(user.UserId, user.Username);
        userToReturn.RefreshToken = GenerateToken(user.UserId, user.Username, true);

        return userToReturn;
    }

    public async Task<UserToReturnDTO> RegisterAsync(UserToRegisterDTO userToRegisterDTO)
    {
        userToRegisterDTO.Username = userToRegisterDTO.Username.ToLower();

        var addedUser = await _userRepository.AddAsync(_mapper.Map<User>(userToRegisterDTO));

        var userToReturn = _mapper.Map<UserToReturnDTO>(addedUser);
        userToReturn.Token = GenerateToken(addedUser.UserId, addedUser.Username);
        userToReturn.RefreshToken = GenerateToken(addedUser.UserId, addedUser.Username, true);

        return userToReturn;
    }

    public RefreshTokenToReturnDTO RefreshToken(RefreshTokenDTO refreshTokenDTO)
    {
        var claimsPrincipal = GetClaimsPrincipal(refreshTokenDTO.RefreshToken);
        if (claimsPrincipal is null)
            throw new BadRequestException();

        var username = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Name)?.FirstOrDefault()?.Value ?? string.Empty;
        var userId = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? string.Empty;
        if (userId == string.Empty || username == string.Empty)
            throw new BadRequestException();

        var refreshToken = GenerateToken(int.Parse(userId), username, true);
        var accessToken = GenerateToken(int.Parse(userId), username);

        return new RefreshTokenToReturnDTO
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