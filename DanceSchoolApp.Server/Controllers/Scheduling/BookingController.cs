using DanceSchoolApp.Server.Services.Scheduling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers.Scheduling
{
    [ApiController]
    [Route("api/ee/classes")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // ─── GET /api/ee/classes/available-slots ──────────────────────────────────
        // Returns free coaching time windows for the given date range, optionally
        // filtered by coach and/or modality. Used by the parent booking calendar.
        [Authorize(Roles = "parent,staff")]
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to,
            [FromQuery] int? coachId = null,
            [FromQuery] int? modalityId = null)
        {
            if (to < from)
                return BadRequest("'to' must be greater than or equal to 'from'.");

            if ((to.DayNumber - from.DayNumber) > 31)
                return BadRequest("Date range must not exceed 31 days.");

            try
            {
                var result = await _bookingService.GetAvailableSlotsAsync(
                    from, to, coachId, modalityId);

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
