using BgituSec.Api.Models.Users;
using FluentValidation;

namespace BgituSec.Application.Features.Users.Validators
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(LoginUserCommand =>
                LoginUserCommand.Name).NotEmpty().MaximumLength(30);
            RuleFor(LoginUserCommand =>
                LoginUserCommand.Password).MinimumLength(8);
        }
    }
}
