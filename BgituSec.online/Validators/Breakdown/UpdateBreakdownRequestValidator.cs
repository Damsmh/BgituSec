using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Repositories;
using FluentValidation;

namespace BgituSec.Api.Validators.Breakdown
{
    public class UpdateBreakdownRequestValidator : AbstractValidator<UpdateBreakdownRequest>
    {
        public UpdateBreakdownRequestValidator(IComputerRepository computerRepository, IUserRepository userRepository)
        {
            RuleFor(CreateBreakdownRequest =>
                CreateBreakdownRequest.IsSolved).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
        }
    }
}
