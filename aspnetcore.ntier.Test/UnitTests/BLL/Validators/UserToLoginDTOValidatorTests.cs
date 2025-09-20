using aspnetcore.ntier.BLL.Validators;
using aspnetcore.ntier.DTO.Dtos;
using FluentValidation.TestHelper;
using Xunit;

namespace aspnetcore.ntier.Test.UnitTests.BLL.Validators;

public class UserToLoginDtoValidatorTests
{
    private readonly UserToLoginDtoValidator _userToLoginDtoValidator;

    public UserToLoginDtoValidatorTests()
    {
        _userToLoginDtoValidator = new UserToLoginDtoValidator();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("username", "")]
    [InlineData("", "password")]
    public void Validate_WhenUsernameOrPasswordOrBothEmpty_ThrowsValidationError(string username, string password)
    {
        var userToLoginDto = new UserToLoginDto
        {
            Username = username,
            Password = password
        };

        var result = _userToLoginDtoValidator.TestValidate(userToLoginDto);
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_WhenUsernameAndPasswordNotEmpty_ShouldNotThrowValidationError()
    {
        var userToLoginDto = new UserToLoginDto
        {
            Username = "username",
            Password = "password"
        };

        var result = _userToLoginDtoValidator.TestValidate(userToLoginDto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
