using DanceSchoolApp.Server.DTOs.School;
using DanceSchoolApp.Server.Services.School;
using Microsoft.AspNetCore.Mvc;


namespace DanceSchoolApp.Server.Controllers.School
{
    [ApiController]
    [Route("api/modalities")]
    public class ModalityController : ControllerBase
    {
        private readonly ModalityService _modalityService;

        public ModalityController(ModalityService modalityService)
        {
            _modalityService = modalityService;
        }

        // ─── GET /api/modalities ───────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetModalities()
        {
            try
            {
                var result = await _modalityService.GetModalitiesAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/modalities/{id} ──────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModality(int id)
        {
            try
            {
                var result = await _modalityService.GetModalityAsync(id);
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

        // ─── POST /api/modalities ──────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> CreateModality([FromBody] ModalityCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _modalityService.CreateModalityAsync(request);
                return CreatedAtAction(nameof(GetModality), new { id = newId }, new { modalityId = newId });
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

        // ─── PATCH /api/modalities/{id} ────────────────────────────────────────
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateModality(int id, [FromBody] ModalityUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _modalityService.UpdateModalityAsync(id, request);
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

        // ─── PATCH /api/modalities/{id}/activate ──────────────────────────────
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateModality(int id)
        {
            try
            {
                await _modalityService.SetModalityStateAsync(id, isActive: true);
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

        // ─── PATCH /api/modalities/{id}/deactivate ────────────────────────────
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateModality(int id)
        {
            try
            {
                await _modalityService.SetModalityStateAsync(id, isActive: false);
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
