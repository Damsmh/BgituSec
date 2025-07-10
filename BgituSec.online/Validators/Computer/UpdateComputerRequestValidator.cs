using BgituSec.Api.Models.Computers.Request;
using BgituSec.Domain.Interfaces;
using FluentValidation;

namespace BgituSec.Api.Validators.Computer
{
    public class UpdateComputerRequestValidator : AbstractValidator<UpdateComputerRequest>
    {
        private readonly IAuditoriumRepository _auditoriumRepository;
        public UpdateComputerRequestValidator(IAuditoriumRepository auditoriumRepository)
        {
            _auditoriumRepository = auditoriumRepository;
            RuleFor(UpdateComputerRequest =>
                UpdateComputerRequest.SerialNumber).NotEmpty();
            RuleFor(UpdateComputerRequest =>
                UpdateComputerRequest.Position).NotEmpty().Must((request, context, cancellationToken) =>
                {
                    var pos = request.Position.Split(';');
                    return double.TryParse(pos[0], out var x) && double.TryParse(pos[1], out var y);
                }).WithMessage("Это не числа.");
            RuleFor(UpdateComputerRequest =>
                UpdateComputerRequest.Size).NotEmpty().Must((request, context, cancellationToken) =>
                {
                    var size = request.Size.Split(";");
                    return double.TryParse(size[0], out var w) && double.TryParse(size[1], out var h);
                }).WithMessage("Это не числа.");
            RuleFor(UpdateComputerRequest =>
                UpdateComputerRequest.Type).InclusiveBetween(0, 3);
            RuleFor(UpdateComputerRequest =>
                UpdateComputerRequest.AuditoriumId).NotEmpty().NotNull()
                .MustAsync(async (request, context, cancellationToken) =>
                {
                    try { await _auditoriumRepository.GetByIdAsync(request.AuditoriumId); return true; }
                    catch (KeyNotFoundException) { return false; }
                }).WithMessage($"Такой AuditoriumId не найден.");
        }
    }
}
