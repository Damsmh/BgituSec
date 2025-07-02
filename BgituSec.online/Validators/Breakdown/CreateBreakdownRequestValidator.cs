using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Domain.Interfaces;
using FluentValidation;

namespace BgituSec.Api.Validators.Breakdown
{
    public class CreateBreakdownRequestValidator : AbstractValidator<CreateBreakdownRequest>
    {
        private readonly IComputerRepository _computerRepository;
        private readonly IUserRepository _userRepository;
        public CreateBreakdownRequestValidator(IComputerRepository computerRepository, IUserRepository userRepository)
        {
            _computerRepository = computerRepository;
            _userRepository = userRepository;
            RuleFor(CreateBreakdownRequest =>
                CreateBreakdownRequest.Description).NotEmpty();
            RuleFor(CreateBreakdownRequest =>
                CreateBreakdownRequest.Level).NotEmpty().NotNull();
            RuleFor(CreateBreakdownRequest =>
                CreateBreakdownRequest.IsSolved).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
            RuleFor(CreateBreakdownRequest => CreateBreakdownRequest.UserId).MustAsync(async (request, context, cancellationToken) =>
            {
                try { var uid = await _userRepository.GetByIdAsync(request.UserId) ?? throw new KeyNotFoundException(); return true; }
                catch (KeyNotFoundException) { return false; }
            }).WithMessage($"Такой UserId не найден.");
            RuleFor(CreateBreakdownRequest => CreateBreakdownRequest.ComputerId).MustAsync(async (request, context, cancellationToken) =>
            {
                try { await _computerRepository.GetByIdAsync(request.ComputerId); return true; }
                catch (KeyNotFoundException) { return false; }
            }).WithMessage($"Такой ComputerId не найден.");
        }
    }
}
