using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Domain.Interfaces;
using FluentValidation;

namespace BgituSec.Api.Validators.Breakdown
{
    public class UpdateBreakdownRequestValidator : AbstractValidator<UpdateBreakdownRequest>
    {
        public UpdateBreakdownRequestValidator()
        {
            
        }
    }
}
