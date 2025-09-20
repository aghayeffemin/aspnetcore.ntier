using aspnetcore.ntier.BLL.Services;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.AutoMapperProfiles;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.BLL.Utilities.Settings;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.Dtos;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace aspnetcore.ntier.Test.UnitTests.BLL.Services;

public class AuthServiceTests
{
    private readonly IAuthService _authService;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly IMapper _mapper;

    private const int UserId = 5;
    private readonly User _userEntity;
    private readonly UserToLoginDto _userToLoginDto;
    private readonly UserToRegisterDto _userToRegisterDto;

    public AuthServiceTests()
    {
        _userEntity = new User()
        {
            UserId = UserId,
            Username = "UserEntityUsername",
            Name = "UserEntityName",
            Surname = "UserEntitySurname",
            Password = "UserEntityPassword"
        };

        _userToLoginDto = new UserToLoginDto()
        {
            Username = "UserToLoginDtoUsername",
            Password = "UserToLoginDtoPassword"
        };

        _userToRegisterDto = new UserToRegisterDto()
        {
            Username = "UserToRegisterDtoUsername",
            Name = "UserToRegisterDtoName",
            Surname = "UserToRegisterDtoSurname",
            Password = "UserToRegisterDtoPassword"
        };

        _userRepository = new Mock<IUserRepository>();

        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        var myProfile = new AutoMapperProfiles.AutoMapperProfile();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(mapperConfiguration);
        var jwtSettingsMock = new JwtSettings
        {
            AccessTokenSecret = "edffbf20-5c7d-4bd1-8752-f4bb2eb8e41fedffbf20-5c7d-4bd1-8752-f4bb2eb8e41f",
            RefreshTokenSecret = "01433849-3cf6-4b57-aad8-21513b29459e01433849-3cf6-4b57-aad8-21513b29459e",
            AccessTokenExpirationMinutes = 1,
            RefreshTokenExpirationMinutes = 1
        };
        var jwtOptions = Options.Create(jwtSettingsMock);

        _authService = new AuthService(_userRepository.Object, _mapper, jwtOptions);
    }

    [Fact]
    public async Task LoginAsync_WhenSuccess_ReturnsUserToReturnDtoWithToken()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _authService.LoginAsync(_userToLoginDto);

        //Assert
        Assert.IsType<UserToReturnDto>(result);
        Assert.Equal(_userEntity.UserId, result.UserId);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync((User)null!);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _authService.LoginAsync(_userToLoginDto));
    }

    [Fact]
    public async Task RegisterAsync_WhenSuccess_RegistersUserThenReturnsUserToReturnDtoWithToken()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _authService.RegisterAsync(_userToRegisterDto);

        //Assert
        Assert.IsType<UserToReturnDto>(result);
        Assert.Equal(_userEntity.UserId, result.UserId);
        Assert.NotEmpty(result.Token);
    }
}
