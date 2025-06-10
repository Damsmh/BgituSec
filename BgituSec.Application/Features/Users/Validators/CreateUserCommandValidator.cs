using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using FluentValidation;

namespace BgituSec.Application.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly Roles[] allRoles = Enum.GetValues<Roles>(); 
        public CreateUserCommandValidator() {
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Name).NotEmpty().MaximumLength(30);
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Email).EmailAddress();
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Password).MinimumLength(8);
            RuleFor(CreateUserCommand =>
                CreateUserCommand.Role)
                .IsInEnum()
                .WithMessage($"Неверная роль пользователя, доступные роли: {allRoles.Select(role => role.ToString())}");
        }
    }
}
