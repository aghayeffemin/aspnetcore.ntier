using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace aspnetcore.ntier.BLL.Services;

public class UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger) : IUserService
{
    public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var usersToReturn = await userRepository.GetListAsync(cancellationToken: cancellationToken);
        logger.LogInformation("List of {Count} users has been returned", usersToReturn.Count);

        return mapper.Map<List<UserDto>>(usersToReturn);
    }

    public async Task<UserDto> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("User with userId = {UserId} was requested", userId);
        var userToReturn = await userRepository.GetAsync(x => x.UserId == userId, cancellationToken);

        if (userToReturn is null)
        {
            logger.LogError("User with userId = {UserId} was not found", userId);
            throw new UserNotFoundException();
        }

        return mapper.Map<UserDto>(userToReturn);
    }

    public async Task<UserDto> AddUserAsync(UserToAddDto userToAddDto)
    {
        userToAddDto.Username = userToAddDto.Username.ToLower();
        var addedUser = await userRepository.AddAsync(mapper.Map<User>(userToAddDto));

        return mapper.Map<UserDto>(addedUser);
    }

    public async Task<UserDto> UpdateUserAsync(UserToUpdateDto userToUpdateDto)
    {
        userToUpdateDto.Username = userToUpdateDto.Username.ToLower();
        var user = await userRepository.GetAsync(x => x.UserId == userToUpdateDto.UserId);

        if (user is null)
        {
            logger.LogError("User with userId = {UserId} was not found", userToUpdateDto.UserId);
            throw new UserNotFoundException();
        }

        var userToUpdate = mapper.Map<User>(userToUpdateDto);

        logger.LogInformation("User with these properties: {@UserToUpdate} has been updated", userToUpdateDto);

        return mapper.Map<UserDto>(await userRepository.UpdateUserAsync(userToUpdate));
    }

    public async Task DeleteUserAsync(int userId)
    {
        var userToDelete = await userRepository.GetAsync(x => x.UserId == userId);

        if (userToDelete is null)
        {
            logger.LogError("User with userId = {UserId} was not found", userId);
            throw new UserNotFoundException();
        }

        await userRepository.DeleteAsync(userToDelete);
    }
}