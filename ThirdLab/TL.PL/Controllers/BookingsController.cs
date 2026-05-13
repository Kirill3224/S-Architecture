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
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request)
    {
        var booking = await _bookingService.CreateAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = booking.Id }, booking);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _bookingService.DeleteAsync(id);
        return Ok(new { message = "Booking was deleted successfully." });
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateBookingRequest request)
    {
        await _bookingService.UpdateAsync(request);
        return Ok(new { message = "Booking was updated successfully." });
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetAvailable([FromQuery] SearchAvailableRoomsRequest request)
    {
        var rooms = await _bookingService.GetAvailableRoomsAsync(request);
        return Ok(rooms);
    }

    [HttpGet("room")]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetByRoom([FromQuery] Guid roomId)
    {
        var booking = await _bookingService.GetBookingsByRoomAsync(roomId);
        return Ok(booking);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }
}