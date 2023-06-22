using aspnetcore.ntier.BLL.Services;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.AutoMapperProfiles;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace aspnetcore.ntier.Test.UnitTests.BLL.Services;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<ILogger<UserService>> _logger;
    private readonly IMapper _mapper;

    private const int UserId = 5;
    private readonly User _userEntity;
    private readonly UserToAddDTO _userToAddDTO;
    private readonly UserToUpdateDTO _userToUpdateDTO;

    public UserServiceTests()
    {
        _userEntity = new User()
        {
            UserId = UserId,
            Username = "UserEntityUsername",
            Name = "UserEntityName",
            Surname = "UserEntitySurname"
        };

        _userToAddDTO = new UserToAddDTO()
        {
            Username = "UserToAddDTOUsername",
            Name = "UserToAddDTOName",
            Surname = "UserToAddDTOSurname"
        };

        _userToUpdateDTO = new UserToUpdateDTO()
        {
            UserId = UserId,
            Username = "UserToUpdateDTOUsername",
            Name = "UserToUpdateDTOName",
            Surname = "UserToUpdateDTOSurname"
        };

        _userRepository = new Mock<IUserRepository>();

        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        _logger = new Mock<ILogger<UserService>>();

        var myProfile = new AutoMapperProfiles.AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);

        _userService = new UserService(_userRepository.Object, _mapper, _logger.Object);
    }

    [Fact]
    public async Task GetUsersAsync_WhenSuccess_ReturnsUserDTOList()
    {
        //Arrange
        var userEntityList = new List<User>() { _userEntity, _userEntity };

        _userRepository
            .Setup(repo => repo.GetListAsync(null!, CancellationToken.None))
            .ReturnsAsync(userEntityList);

        //Act
        var result = await _userService.GetUsersAsync();

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetUserAsync_WhenSuccess_ReturnsUserDTOList()
    {
        //Act
        var result = await _userService.GetUserAsync(UserId);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync((User)null!);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserAsync(UserId));
    }

    [Fact]
    public async Task AddUserAsync_WhenSuccess_AddsThenReturnsUserDTO()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _userService.AddUserAsync(_userToAddDTO);

        //Assert
        Assert.IsType<UserDTO>(result);
        Assert.Equal(_userEntity.UserId, result.UserId);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenSuccess_UpdatesThenReturnsUserDTO()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.UpdateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _userService.UpdateUserAsync(_userToUpdateDTO);

        //Assert
        Assert.IsType<UserDTO>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync((User)null!);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateUserAsync(_userToUpdateDTO));
    }

    [Fact]
    public async Task DeleteUserAsync_WhenSuccess_CallsRepositoryDelete()
    {
        //Act
        await _userService.DeleteUserAsync(UserId);

        //Assert
        _userRepository.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once());
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
            .ReturnsAsync((User)null!);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteUserAsync(UserId));
    }
}
