using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using FluentValidation;

namespace BgituSec.Application.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator() {
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Name).NotEmpty().MaximumLength(50);
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Email).EmailAddress();
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Password).NotEmpty().Length(8, 20);
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Role).NotEmpty().IsInEnum();
        }
    }
}
