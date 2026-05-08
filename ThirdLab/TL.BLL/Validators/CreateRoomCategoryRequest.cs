using FluentValidation;
using TL.BLL.DTOs;

namespace TL.BLL.Validators.Room;

public class CreateRoomCategoryValidator : AbstractValidator<CreateRoomCategoryRequest>
{
    public CreateRoomCategoryValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(r => r.PricePerNight)
            .GreaterThan(0)
            .WithMessage("Price per night must be greater than 0");
    }
}
