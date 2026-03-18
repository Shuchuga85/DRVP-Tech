using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers
{
    [Route("api/[controller]es")]
    [ApiController]
    public class CoachController : ControllerBase
    {

        public readonly CoachService _CoachService;
        public CoachController(CoachService CoachService)
        {
            _CoachService = CoachService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoachs()
        {
            try
            {
                var result = await _CoachService.GetCoachsAsync();

                if (result is null || !result.Any())
                {
                    return NotFound();

                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoach(int id)
        {
            try
            {
                var result = await _CoachService.GetCoachAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        /*
        [HttpPost]
        public async Task<IActionResult> CreateCoach([FromBody] CoachCreateRequest request)
        {
            try
            {
                var result = await _CoachService.CreateCoachAsync(request);

                if (!result)
                    return BadRequest("Coach failed to create");

                return Ok("New Coach added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateCoach(int id)
        {
            try
            {
                var result = await _CoachService.SetCoachStateAsync(new CoachActivationRequest { CoachId = id, IsActive = true });

                if (!result)
                    return BadRequest("Failed to activate Coach");

                return Ok("Coach Activated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateCoach(int id)
        {
            try
            {
                var result = await _CoachService.SetCoachStateAsync(new CoachActivationRequest { CoachId = id, IsActive = false });

                if (!result)
                    return BadRequest("Failed to deactivate Coach");

                return Ok("Coach Deactivated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        */
    }
}
