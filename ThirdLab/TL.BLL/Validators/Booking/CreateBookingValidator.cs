using FluentValidation;
using TL.BLL.DTOs;

namespace TL.BLL.Validators.Booking;

public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingValidator()
    {
        RuleFor(b => b.RoomId).NotEmpty();

        RuleFor(b => b.GuestName)
            .NotEmpty().WithMessage("Guest name cannot be empty.")
            .MaximumLength(100).WithMessage("Guest name must not exceed 100 characters");

        RuleFor(b => b.StartDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .NotEmpty().WithMessage("Start date cannot be empty.");

        RuleFor(b => b.EndDate)
            .GreaterThan(b => b.StartDate)
            .NotEmpty().WithMessage("End date cannot be empty.");
    }
}
