using aspnetcore.ntier.DTO.Dtos;
using FluentValidation;

namespace aspnetcore.ntier.BLL.Validators;

public class UserToLoginDtoValidator : AbstractValidator<UserToLoginDto>
{
    public UserToLoginDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
