using aspnetcore.ntier.BLL.Services;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.AutoMapperProfiles;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using Microsoft.Extensions.Configuration;
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
    private readonly UserToLoginDTO _userToLoginDTO;
    private readonly UserToRegisterDTO _userToRegisterDTO;

    public AuthServiceTests()
    {
        _userEntity = new User()
        {
            UserId = UserId,
            Username = "UserEntityUsername",
            Name = "UserEntityName",
            Surname = "UserEntitySurname"
        };

        _userToLoginDTO = new UserToLoginDTO()
        {
            Username = "UserToLoginDTOUsername",
            Password = "UserToLoginDTOPassword"
        };

        _userToRegisterDTO = new UserToRegisterDTO()
        {
            Username = "UserToRegisterDTOUsername",
            Name = "UserToRegisterDTOName",
            Surname = "UserToRegisterDTOSurname"
        };

        _userRepository = new Mock<IUserRepository>();

        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        var myProfile = new AutoMapperProfiles.AutoMapperProfile();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(mapperConfiguration);

        var configurationSectionMock = new Mock<IConfigurationSection>();
        var configurationMock = new Mock<IConfiguration>();
        configurationSectionMock
           .Setup(x => x.Value)
           .Returns("Superb token for testing purposes");
        configurationMock
           .Setup(x => x.GetSection("AppSettings:Token"))
           .Returns(configurationSectionMock.Object);

        _authService = new AuthService(_userRepository.Object, _mapper, configurationMock.Object);
    }

    [Fact]
    public async Task LoginAsync_WhenSuccess_ReturnsUserToReturnDTOWithToken()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _authService.LoginAsync(_userToLoginDTO);

        //Assert
        Assert.IsType<UserToReturnDTO>(result);
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
        await Assert.ThrowsAsync<UserNotFoundException>(() => _authService.LoginAsync(_userToLoginDTO));
    }

    [Fact]
    public async Task RegisterAsync_WhenSuccess_RegistersUserThenReturnsUserToReturnDTOWithToken()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _authService.RegisterAsync(_userToRegisterDTO);

        //Assert
        Assert.IsType<UserToReturnDTO>(result);
        Assert.Equal(_userEntity.UserId, result.UserId);
        Assert.NotEmpty(result.Token);
    }
}
