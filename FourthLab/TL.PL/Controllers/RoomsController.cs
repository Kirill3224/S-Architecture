using Microsoft.AspNetCore.Mvc;
using TL.BLL.DTOs;
using TL.BLL.Interfaces;

namespace TL.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    public async Task<ActionResult<RoomResponse>> Create(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var id = await _roomService.CreateAsync(request, cancellationToken);

        var roomResponse = await _roomService.GetByIdAsync(id, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, roomResponse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _roomService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        await _roomService.UpdateAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var room = await _roomService.GetByIdAsync(id, cancellationToken);
        return Ok(room);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var rooms = await _roomService.GetAllAsync(cancellationToken);
        return Ok(rooms);
    }
}