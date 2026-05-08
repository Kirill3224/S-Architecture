using FluentValidation;
using TL.BLL.DTOs;
using TL.DAL.Enums;

namespace TL.BLL.Validators.Room;

public class UpdateRoomValidator : AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Number).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Status).IsEnumName(typeof(RoomStatus), caseSensitive: false)
                .WithMessage($"Invalid Status. Allowed: {string.Join(", ", Enum.GetNames(typeof(RoomStatus)))}");
    }
}
