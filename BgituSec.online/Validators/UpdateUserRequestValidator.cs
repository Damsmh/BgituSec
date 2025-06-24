using BgituSec.Api.Models.Users.Request;
using BgituSec.Domain.Interfaces;
using FluentValidation;
using System.Xml.Linq;

namespace BgituSec.Api.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserRequestValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Name).NotEmpty().MaximumLength(30)
                .MustAsync(async (request, context, cancellationToken) => await _userRepository.IsUserNameExist(request.Name))
                .WithMessage("Пользователь с таким именем уже существует.");
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Email).EmailAddress();
            RuleFor(CreateUserRequest =>
                CreateUserRequest.Password).MinimumLength(8);
        }
    }
}
