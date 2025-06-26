using BgituSec.Api.Models.Buildings.Request;
using FluentValidation;

namespace BgituSec.Api.Validators
{
    public class BuildingRequestValidator : AbstractValidator<BuildingRequest>
    {
        public BuildingRequestValidator() {
            RuleFor(buildingRequest => buildingRequest.Number).NotNull().InclusiveBetween(1, int.MaxValue);
            RuleFor(buildingRequest => buildingRequest.Floors).NotNull().InclusiveBetween(1, int.MaxValue);
        }
    }
}
