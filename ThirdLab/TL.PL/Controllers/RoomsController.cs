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
    public async Task<ActionResult<RoomResponse>> Create(CreateRoomRequest request)
    {
        var id = await _roomService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _roomService.DeleteAsync(id);
        return Ok(new { message = "Room deleted successfully." });
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateRoomRequest request)
    {
        await _roomService.UpdateAsync(request);
        return Ok(new { message = "Room updated successfully" });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomResponse>> GetById(Guid id)
    {
        var room = await _roomService.GetByIdAsync(id);
        return Ok(room);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();
        return Ok(rooms);
    }
}