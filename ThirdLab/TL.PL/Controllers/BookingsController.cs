using Microsoft.AspNetCore.Mvc;
using TL.BLL.DTOs;
using TL.BLL.Interfaces;

namespace TL.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _bookingService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateBookingRequest request, CancellationToken cancellationToken)
    {
        await _bookingService.UpdateAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAvailable([FromQuery] SearchAvailableRoomsRequest request, CancellationToken cancellationToken)
    {
        var rooms = await _bookingService.GetAvailableRoomsAsync(request, cancellationToken);
        return Ok(rooms);
    }

    [HttpGet("room/{roomId:guid}")]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetByRoom([FromRoute] Guid roomId, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.GetBookingsByRoomAsync(roomId, cancellationToken);
        return Ok(booking);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookingResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.GetByIdAsync(id, cancellationToken);
        return Ok(booking);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var bookings = await _bookingService.GetAllAsync(cancellationToken);
        return Ok(bookings);
    }
}