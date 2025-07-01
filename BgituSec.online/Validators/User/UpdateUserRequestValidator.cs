using BgituSec.Api.Models.Users.Request;
using BgituSec.Domain.Interfaces;
using FluentValidation;

namespace BgituSec.Api.Validators.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserByIdRequest>
    {
        private readonly IUserRepository _repository;
        public UpdateUserRequestValidator(IUserRepository repository) {
            _repository = repository;
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Name).NotEmpty().MaximumLength(30)
                .MustAsync(async (request, context, cancellationToken) =>
                !await _repository.IsUserNameExist(request.Name, request.Id))
                .WithMessage("Пользователь с таким именем уже существует.");
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Email).EmailAddress();
            RuleFor(UpdateUserRequest =>
                UpdateUserRequest.Password).MinimumLength(8);
            RuleFor(UpdateUserRequest => UpdateUserRequest.SentNotifications).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
        }
    }
}
