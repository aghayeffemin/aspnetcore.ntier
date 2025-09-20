using aspnetcore.ntier.BLL.Validators;
using aspnetcore.ntier.DTO.Dtos;
using FluentValidation.TestHelper;
using Xunit;

namespace aspnetcore.ntier.Test.UnitTests.BLL.Validators;

public class UserToRegisterDtoValidatorTests
{
    private readonly UserToRegisterDtoValidator _userToRegisterDtoValidator;

    public UserToRegisterDtoValidatorTests()
    {
        _userToRegisterDtoValidator = new UserToRegisterDtoValidator();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("username", "")]
    [InlineData("", "password")]
    public void Validate_WhenUsernameOrPasswordOrBothEmpty_ThrowsValidationError(string username, string password)
    {
        var userToRegisterDto = new UserToRegisterDto
        {
            Username = username,
            Password = password,
            Name = "",
            Surname = ""
        };

        var result = _userToRegisterDtoValidator.TestValidate(userToRegisterDto);
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_WhenUsernameAndPasswordNotEmpty_ShouldNotThrowValidationError()
    {
        var userToRegisterDto = new UserToRegisterDto
        {
            Username = "username",
            Password = "password",
            Name = "",
            Surname = ""
        };

        var result = _userToRegisterDtoValidator.TestValidate(userToRegisterDto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

