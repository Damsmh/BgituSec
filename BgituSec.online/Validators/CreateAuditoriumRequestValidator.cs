using BgituSec.Api.Models.Auditoriums.Request;
using FluentValidation;

namespace BgituSec.Api.Validators
{
    public class CreateAuditoriumRequestValidator : AbstractValidator<CreateAuditoriumRequest>
    {
        public CreateAuditoriumRequestValidator()
        {
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Position).NotEmpty().Must((request, context, cancellationToken) =>
                {
                    var pos = request.Position.Split(';');
                    return int.TryParse(pos[0], out var x) && int.TryParse(pos[1], out var y);
                }).WithMessage("Это не числа/не целые числа.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Size).NotEmpty().Must((request, context, cancellationToken) =>
                {
                    var size = request.Size.Split("*");
                    return int.TryParse(size[0], out var w) && int.TryParse(size[1], out var h);
                }).WithMessage("Это не числа/не целые числа.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.BuildingId).NotEmpty().NotNull();
            RuleFor(auditoriumRequest =>
                auditoriumRequest.IsComputer).NotNull().WithMessage("Поддерживаются только булевы значения true/false.");
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Floor).NotEmpty().NotNull();
            RuleFor(auditoriumRequest =>
                auditoriumRequest.Name).NotEmpty();

        }
    }
}
