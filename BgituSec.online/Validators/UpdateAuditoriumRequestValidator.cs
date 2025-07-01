using BgituSec.Api.Models.Auditoriums.Request;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Repositories;
using FluentValidation;

namespace BgituSec.Api.Validators
{
    public class UpdateAuditoriumRequestValidator : AbstractValidator<UpdateAuditoriumRequest>
    {
        private readonly IBuildingRepository _buildingRepository;
        public UpdateAuditoriumRequestValidator(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Position).NotEmpty().Must((request, context, cancellationToken) =>
                {
                    var pos = request.Position.Split(';');
                    return double.TryParse(pos[0], out var x) && double.TryParse(pos[1], out var y);
                }).WithMessage("Это не числа.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Size).NotEmpty().Must((request, context, cancellationToken) =>
                {
                    var size = request.Size.Split("*");
                    return int.TryParse(size[0], out var w) && int.TryParse(size[1], out var h);
                }).WithMessage("Это не числа/не целые числа.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.BuildingId).NotEmpty().NotNull()
                .MustAsync(async (request, context, cancellationToken) =>
                {
                    try { await _buildingRepository.GetByIdAsync(request.BuildingId); return true; }
                    catch (KeyNotFoundException) { return false; }
                }).WithMessage($"Такой BuildingId не найден.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.IsComputer).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Floor).NotEmpty().NotNull();
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Name).NotEmpty();
        }
    }
}
