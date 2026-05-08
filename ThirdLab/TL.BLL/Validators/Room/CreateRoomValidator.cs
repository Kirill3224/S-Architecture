using FluentValidation;
using TL.BLL.DTOs;
using TL.DAL.Enums;

namespace TL.BLL.Validators.Room;

public class CreateRoomValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomValidator()
    {
        RuleFor(r => r.Number).NotEmpty().MaximumLength(50);
        RuleFor(r => r.CategoryId).NotEmpty();
        RuleFor(r => r.Status).IsEnumName(typeof(RoomStatus), caseSensitive: false)
                .WithMessage($"Invalid Status. Allowed: {string.Join(", ", Enum.GetNames(typeof(RoomStatus)))}");
    }
}
