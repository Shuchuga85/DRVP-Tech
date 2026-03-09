using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        public readonly StaffService _StaffService;
        public StaffController(StaffService StaffService)
        {
            _StaffService = StaffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {
                var result = await _StaffService.GetStaffsAsync();

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
        public async Task<IActionResult> GetStaff(int id)
        {
            try
            {
                var result = await _StaffService.GetStaffAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] StaffCreateRequest request)
        {
            try
            {
                var result = await _StaffService.CreateStaffAsync(request);

                if (!result)
                    return BadRequest("Staff failed to create");

                return Ok("New Staff added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateStaff(int id)
        {
            try
            {
                var result = await _StaffService.SetStaffStateAsync(new StaffActivationRequest { StaffId = id, IsActive = true });

                if (!result)
                    return BadRequest("Failed to activate Staff");

                return Ok("Staff Activated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateStaff(int id)
        {
            try
            {
                var result = await _StaffService.SetStaffStateAsync(new StaffActivationRequest { StaffId = id, IsActive = false });

                if (!result)
                    return BadRequest("Failed to deactivate Staff");

                return Ok("Staff Deactivated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
