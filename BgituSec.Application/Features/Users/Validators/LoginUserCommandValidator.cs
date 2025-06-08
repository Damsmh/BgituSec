using BgituSec.Application.Features.Users.Commands;
using FluentValidation;

namespace BgituSec.Application.Features.Users.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(LoginUserCommand =>
                LoginUserCommand.Name).NotEmpty().MaximumLength(50);
            RuleFor(LoginUserCommand =>
                LoginUserCommand.Password).NotEmpty().MinimumLength(8);
        }
    }
}
