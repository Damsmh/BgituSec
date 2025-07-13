using BgituSec.Api.Models.Users.Request;
using FluentValidation;

namespace BgituSec.Api.Validators.User
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
