using FluentValidation;
using TL.BLL.DTOs;

namespace TL.BLL.Validators;

public class SearchAvailableRoomsValidator : AbstractValidator<SearchAvailableRoomsRequest>
{
    public SearchAvailableRoomsValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date required.")
            .NotEqual(default(DateTime)).WithMessage("Provide correct start date.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .NotEqual(default(DateTime)).WithMessage("Provide correct end date.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be later than Start date.");
    }
}