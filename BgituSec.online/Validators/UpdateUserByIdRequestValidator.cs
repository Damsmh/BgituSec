using BgituSec.Api.Models.Users.Request;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using FluentValidation;

namespace BgituSec.Api.Validators
{
    public class UpdateUserByIdRequestValidator : AbstractValidator<UpdateUserByIdRequest>
    {
        private readonly Roles[] allRoles = Enum.GetValues<Roles>();
        private readonly IUserRepository _userRepository;
        
        public UpdateUserByIdRequestValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Name).NotEmpty().MaximumLength(30)
                .MustAsync(async (request, context, cancellationToken) => 
                !await _userRepository.IsUserNameExist(request.Name, request.Id))
                .WithMessage("Пользователь с таким именем уже существует.");
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Email).EmailAddress();
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Password).MinimumLength(8);
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Role)
                .IsInEnum()
                .WithMessage($"Неверная роль пользователя, доступные роли: {allRoles.Select(role => role.ToString())}");
            RuleFor(UpdateUserRequest => UpdateUserRequest.SentNotifications).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
        }
    }
}
