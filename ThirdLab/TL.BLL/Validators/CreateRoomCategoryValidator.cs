using FluentValidation;
using TL.BLL.DTOs;

namespace TL.BLL.Validators.RoomCategory;

public class CreateRoomCategoryValidator : AbstractValidator<CreateRoomCategoryRequest>
{
    public CreateRoomCategoryValidator()
    {
        RuleFor(rc => rc.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(rc => rc.PricePerNight)
            .GreaterThan(0)
            .WithMessage("Price per night must be greater than 0");
    }
}
