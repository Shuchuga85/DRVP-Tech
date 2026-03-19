using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services.People;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers.People
{
    [Route("api/[controller]es")]
    [ApiController]
    public class CoachController : ControllerBase
    {

        private readonly CoachService _CoachService;
        public CoachController(CoachService CoachService)
        {
            _CoachService = CoachService;
        }

        // ─── GET /api/coaches ──────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetCoachs()
        {
            try
            {
                var result = await _CoachService.GetCoachsAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/coaches/{id} ─────────────────────────────────────────────
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoach(int id)
        {
            try
            {
                var result = await _CoachService.GetCoachAsync(id);
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
       
    }
}
