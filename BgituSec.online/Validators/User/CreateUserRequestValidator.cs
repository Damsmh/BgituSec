using BgituSec.Api.Models.Users.Request;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using FluentValidation;

namespace BgituSec.Api.Validators.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly Roles[] allRoles = Enum.GetValues<Roles>(); 
        private readonly IUserRepository _userRepository;
        public CreateUserRequestValidator(IUserRepository userRepository) {
            _userRepository = userRepository;
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Name).NotEmpty().MaximumLength(30)
                .MustAsync(async (request, context, cancellationToken) => !await _userRepository.IsUserNameExist(username: request.Name))
                .WithMessage("Пользователь с таким именем уже существует.");
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Email).EmailAddress();
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Password).MinimumLength(8);
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Role)
                .IsInEnum()
                .WithMessage($"Неверная роль пользователя, доступные роли: {allRoles.Select(role => role.ToString())}");
            RuleFor(CreateUserRequest =>
                CreateUserRequest.SentNotifications).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
        }
    }
}
