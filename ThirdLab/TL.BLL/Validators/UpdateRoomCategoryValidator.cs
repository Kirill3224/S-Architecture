using FluentValidation;
using TL.BLL.DTOs;

namespace TL.BLL.Validators.RoomCategory;

public class UpdateRoomCategoryValidator : AbstractValidator<UpdateRoomCategoryRequest>
{
    public UpdateRoomCategoryValidator()
    {
        RuleFor(rc => rc.Id).NotEmpty();

        RuleFor(rc => rc.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(rc => rc.PricePerNight)
            .GreaterThan(0)
            .WithMessage("Price per night must be greater than 0");
    }
}
