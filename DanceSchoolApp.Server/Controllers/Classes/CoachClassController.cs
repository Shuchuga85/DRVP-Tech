using DanceSchoolApp.Server.DTOs.Classes;
using DanceSchoolApp.Server.Services.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DanceSchoolApp.Server.Controllers.Classes
{
    [ApiController]
    [Route("api/coachclasses")]
    public class CoachClassController : ControllerBase
    {
        private readonly CoachClassService _coachClassService;

        public CoachClassController(CoachClassService coachClassService)
        {
            _coachClassService = coachClassService;
        }

        // ─── GET /api/coachclasses ─────────────────────────────────────────────
        // Staff use — returns all classes regardless of status.
        [Authorize(Roles = "staff")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _coachClassService.GetAllAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/coachclasses/{id} ────────────────────────────────────────
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _coachClassService.GetByIdAsync(id);
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

        // ─── GET /api/coachclasses/open ────────────────────────────────────────
        // Parent use — returns Approved classes with available spots.
        [Authorize(Roles = "staff,parent")]
        [HttpGet("open")]
        public async Task<IActionResult> GetOpenClasses()
        {
            try
            {
                var result = await _coachClassService.GetOpenClassesAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/coachclasses/status/{status} ─────────────────────────────
        // Staff use — filter classes by status.
        // Status values: 0=Requested, 1=Approved, 2=Rejected,
        //                3=Cancelled, 4=Finished, 5=Validated, 6=Pending
        [Authorize(Roles = "staff")]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(byte status)
        {
            if (!Enum.IsDefined(typeof(CoachClassStatus), status))
                return BadRequest($"Invalid status value '{status}'. " +
                    "Valid values: 0=Requested, 1=Approved, 2=Rejected, " +
                    "3=Cancelled, 4=Finished, 5=Validated, 6=Pending.");

            try
            {
                var result = await _coachClassService
                    .GetByStatusAsync((CoachClassStatus)status);

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/coachclasses/parent/{parentUserId} ───────────────────────
        // Parent use — returns all classes where this parent's students are enrolled.
        [Authorize(Roles = "staff,parent")]
        [HttpGet("parent/{parentUserId}")]
        public async Task<IActionResult> GetByParent(int parentUserId)
        {
            try
            {
                var result = await _coachClassService.GetByParentAsync(parentUserId);

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

        // ─── POST /api/coachclasses ────────────────────────────────────────────
        // Parent use — creates a class request with at least one student.
        // Runs all conflict checks before inserting.
        [Authorize(Roles = "staff,parent")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CoachClassCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _coachClassService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = newId },
                    new { classId = newId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/coachclasses/{id}/approve ─────────────────────────────
        // Staff use — transitions Requested → Approved.
        [Authorize(Roles = "staff")]
        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _coachClassService.ApproveAsync(id);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/coachclasses/{id}/reject ──────────────────────────────
        // Staff use — transitions Requested → Rejected.
        // Optional reason body is forwarded to notification (TODO).
        [Authorize(Roles = "staff")]
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> Reject(int id,
            [FromBody] CoachClassRejectRequest? request)
        {
            try
            {
                await _coachClassService.RejectAsync(id, request?.Reason);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/coachclasses/{id}/cancel ──────────────────────────────
        // Parent or staff use — transitions Requested or Approved → Cancelled.
        [Authorize(Roles = "staff")]
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _coachClassService.CancelAsync(id);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/coachclasses/{id}/finish ──────────────────────────────
        // Staff use — transitions Approved → Finished, triggering the
        // 48h validation window for coach and parent.
        [Authorize(Roles = "staff")]
        [HttpPatch("{id}/finish")]
        public async Task<IActionResult> Finish(int id)
        {
            try
            {
                await _coachClassService.FinishAsync(id);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}