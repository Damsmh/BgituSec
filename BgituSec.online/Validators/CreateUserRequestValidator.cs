using BgituSec.Api.Models.Users;
using BgituSec.Domain.Entities;
using FluentValidation;

namespace BgituSec.Application.Features.Users.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly Roles[] allRoles = Enum.GetValues<Roles>(); 
        public CreateUserRequestValidator() {
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Name).NotEmpty().MaximumLength(30);
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Email).EmailAddress();
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Password).MinimumLength(8);
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Role)
                .IsInEnum()
                .WithMessage($"Неверная роль пользователя, доступные роли: {allRoles.Select(role => role.ToString())}");
        }
    }
}
