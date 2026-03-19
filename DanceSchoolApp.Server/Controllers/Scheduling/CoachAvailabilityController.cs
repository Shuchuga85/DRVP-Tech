using DanceSchoolApp.Server.DTOs.Scheduling;
using DanceSchoolApp.Server.Services.Scheduling;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers.Scheduling
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CoachAvailabilityController : ControllerBase
    {
        private readonly CoachAvailabilityService _availabilityService;

        public CoachAvailabilityController(CoachAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        // ─── GET /api/coachavailability ────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _availabilityService.GetAllAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/coachavailability/{id} ───────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _availabilityService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/coachavailability/coach/{coachId} ────────────────────────
        // Returns all weekly availability slots defined for a specific coach.
        // The complex "what slots are free on date X" query belongs in
        // BookingController once CoachClass is built.
        [HttpGet("coach/{coachId}")]
        public async Task<IActionResult> GetByCoach(int coachId)
        {
            try
            {
                var result = await _availabilityService.GetByCoachAsync(coachId);

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── POST /api/coachavailability ───────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CoachAvailabilityCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _availabilityService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = newId }, new { coachAvId = newId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PUT /api/coachavailability/{id} ───────────────────────────────────
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CoachAvailabilityUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _availabilityService.UpdateAsync(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── DELETE /api/coachavailability/{id} ────────────────────────────────
        // Hard delete — availability slots are configuration data, not
        // transactional records, so soft delete is not needed here.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _availabilityService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
