using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        public readonly ParentService _ParentService;
        public ParentController(ParentService ParentService)
        {
            _ParentService = ParentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetParents()
        {
            try
            {
                var result = await _ParentService.GetParentsAsync();

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
        public async Task<IActionResult> GetParent(int id)
        {
            try
            {
                var result = await _ParentService.GetParentAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateParent([FromBody] ParentCreateRequest request)
        {
            try
            {
                var result = await _ParentService.CreateParentAsync(request);

                if (!result)
                    return BadRequest("Parent failed to create");

                return Ok("New Parent added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateParent(int id)
        {
            try
            {
                var result = await _ParentService.SetParentStateAsync(new ParentActivationRequest { ParentId = id, IsActive = true });

                if (!result)
                    return BadRequest("Failed to activate parent");

                return Ok("Parent Activated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateParent(int id)
        {
            try
            {
                var result = await _ParentService.SetParentStateAsync(new ParentActivationRequest { ParentId = id, IsActive = false });

                if (!result)
                    return BadRequest("Failed to deactivate parent");

                return Ok("Parent Deactivated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
