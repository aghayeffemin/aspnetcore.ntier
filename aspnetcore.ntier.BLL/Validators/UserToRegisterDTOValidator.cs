using aspnetcore.ntier.DTO.Dtos;
using FluentValidation;

namespace aspnetcore.ntier.BLL.Validators;

public class UserToRegisterDtoValidator : AbstractValidator<UserToRegisterDto>
{
    public UserToRegisterDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

