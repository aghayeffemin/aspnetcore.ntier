using aspnetcore.ntier.DTO.DTOs;
using FluentValidation;

namespace aspnetcore.ntier.BLL.Validators;

public class UserToRegisterDTOValidator : AbstractValidator<UserToRegisterDTO>
{
    public UserToRegisterDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

