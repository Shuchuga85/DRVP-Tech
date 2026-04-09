using DanceSchoolApp.Server.DTOs.Social;
using DanceSchoolApp.Server.Services.Social;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DanceSchoolApp.Server.Controllers.Social
{
    [ApiController]
    [Route("api/newsposts")]
    public class NewsPostController : ControllerBase
    {
        private readonly NewsPostService _newsPostService;

        public NewsPostController(NewsPostService newsPostService)
        {
            _newsPostService = newsPostService;
        }

        // ─── GET /api/newsposts ────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _newsPostService.GetAllAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/newsposts/{id} ───────────────────────────────────────────
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _newsPostService.GetByIdAsync(id);
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

        // ─── POST /api/newsposts ───────────────────────────────────────────────
        // Staff use — publish a new news post.
        [Authorize(Roles = "staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsPostCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _newsPostService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = newId },
                    new { postId = newId });
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

        // ─── PUT /api/newsposts/{id} ───────────────────────────────────────────
        // Staff use — update post content. CreatedBy and CreatedAt are immutable.
        [Authorize(Roles = "staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NewsPostUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _newsPostService.UpdateAsync(id, request);
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

        // ─── DELETE /api/newsposts/{id} ────────────────────────────────────────
        // Staff use — hard delete. News posts have no billing or audit dependency.
        [Authorize(Roles = "staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _newsPostService.DeleteAsync(id);
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