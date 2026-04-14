using DanceSchoolApp.Server.Services.People;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DanceSchoolApp.Server.Controllers.People
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly ParentService _parentService;
        public ParentController(ParentService ParentService)
        {
            _parentService = ParentService;
        }

        // ─── GET /api/parents ─────────────────────────────────────────────────
        [Authorize(Roles = "staff")]
        [HttpGet]
        public async Task<IActionResult> GetParents()
        {
            try
            {
                var result = await _parentService.GetParentsAsync();

                if (!result.Any())
                    return NoContent();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/parents/{id} ─────────────────────────────────────────────────
        [Authorize(Roles = "staff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParent(int id)
        {
            try
            {
                var result = await _parentService.GetParentAsync(id);
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
