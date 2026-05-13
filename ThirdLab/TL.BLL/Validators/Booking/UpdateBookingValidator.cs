using FluentValidation;
using TL.BLL.DTOs;

namespace TL.BLL.Validators.Booking;

public class UpdateBookingValidator : AbstractValidator<UpdateBookingRequest>
{
    public UpdateBookingValidator()
    {
        RuleFor(b => b.Id).NotEmpty();

        RuleFor(b => b.GuestName)
            .NotEmpty().WithMessage("Guest name cannot be empty.")
            .MaximumLength(100).WithMessage("Guest name must not exceed 100 characters")
            .When(x => x.GuestName != null);

        RuleFor(b => b.StartDate)
            .NotEmpty().WithMessage("Start date cannot be empty.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage($"Start date should be greater, or equal to {DateTime.UtcNow.Date}.")
            .Unless(x => x.StartDate == null);

        RuleFor(b => b.EndDate)
            .NotEmpty().WithMessage("End date cannot be empty.")
            .GreaterThan(b => b.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue).WithMessage("End date should be greater than Start date.")
            .Unless(x => x.EndDate == null);
    }
}
