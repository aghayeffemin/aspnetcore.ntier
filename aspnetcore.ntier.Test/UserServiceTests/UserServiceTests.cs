using aspnetcore.ntier.BLL.Services;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.AutoMapperProfiles;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using aspnetcore.ntier.Entity.Entities;
using AutoMapper;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace aspnetcore.ntier.Test.UserServiceTests;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly IMapper _mapper;

    private const int UserId = 5;
    private readonly User _userEntity;
    private readonly UserDTO _userDTO;
    private readonly UserToAddDTO _userToAddDTO;

    public UserServiceTests()
    {
        _userEntity = new User()
        {
            UserId = UserId,
            Username = "UserEntityUserName",
            Name = "UserEntityName",
            Surname = "UserEntitySurName"
        };

        _userDTO = new UserDTO()
        {
            UserId = UserId,
            Username = "UserDTOUserName",
            Name = "UserDTOName",
            Surname = "UserDTOSurName"
        };

        _userToAddDTO = new UserToAddDTO()
        {
            Username = "UserToAddDTOUserName",
            Name = "UserToAddDTOName",
            Surname = "UserToAddDTOSurName"
        };

        _userRepository = new Mock<IUserRepository>();

        _userRepository
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(_userEntity);

        var myProfile = new AutoMapperProfiles.AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);

        _userService = new UserService(_userRepository.Object, _mapper);
    }

    [Fact]
    public async Task GetUsers_ReturnsUserList()
    {
        //Arrange
        var userEntityList = new List<User>() { _userEntity, _userEntity };

        _userRepository
            .Setup(repo => repo.GetList(null))
            .ReturnsAsync(userEntityList);

        //Act
        var result = await _userService.GetUsers();

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetUser_ReturnsUser()
    {
        //Act
        var result = await _userService.GetUser(UserId);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUser_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUser(UserId));
    }

    [Fact]
    public async Task AddUser_ReturnsUser()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.Add(It.IsAny<User>()))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _userService.AddUser(_userToAddDTO);

        //Assert
        Assert.IsType<UserDTO>(result);
        Assert.Equal(_userEntity.UserId, result.UserId);
    }

    [Fact]
    public async Task UpdateUser_ReturnsUser()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.Update(It.IsAny<User>()))
            .ReturnsAsync(_userEntity);

        //Act
        var result = await _userService.UpdateUser(_userDTO);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateUser(_userDTO));
    }

    [Fact]
    public async Task DeleteUser_CallsRepositoryDelete()
    {
        //Act
        await _userService.DeleteUser(UserId);

        //Assert
        _userRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Once());
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        //Arrange
        _userRepository
            .Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        //Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteUser(UserId));
    }
}
