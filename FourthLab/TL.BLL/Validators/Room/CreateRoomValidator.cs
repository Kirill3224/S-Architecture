using FluentValidation;
using TL.BLL.DTOs;
using TL.DAL.Enums;

namespace TL.BLL.Validators.Room;

public class CreateRoomValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomValidator()
    {
        RuleFor(r => r.Number)
            .NotEmpty().WithMessage("Number cannot be empty.")
            .MaximumLength(50).WithMessage("Number must not exceed 50 characters");

        RuleFor(r => r.CategoryId)
            .NotEmpty().WithMessage("Category ID cannot be empty.");

        RuleFor(r => r.Status).IsEnumName(typeof(RoomStatus), caseSensitive: false)
                .WithMessage($"Invalid Status. Allowed: {string.Join(", ", Enum.GetNames(typeof(RoomStatus)))}");
    }
}
