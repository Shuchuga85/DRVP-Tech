using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.DTOs.Inventory;
using DanceSchoolApp.Server.Services.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DanceSchoolApp.Server.Controllers.Inventory
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        // ─── Helper ───────────────────────────────────────────────────────────────

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsStaff() =>
            User.IsInRole("staff");

        // ═══════════════════════════════════════════════════════════════════════
        // ITEMS
        // ═══════════════════════════════════════════════════════════════════════

        // ─── GET /api/items ───────────────────────────────────────────────────
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetItems(
            [FromQuery] bool? fromSchool = null,
            [FromQuery] PagedQuery? query = null)
        {
            try
            {
                var result = await _itemService.GetItemsAsync(fromSchool, query ?? new PagedQuery());

                if (result.TotalCount == 0)
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/items/{id} ──────────────────────────────────────────────
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var result = await _itemService.GetItemAsync(id);
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

        // ─── POST /api/items/school ───────────────────────────────────────────
        /// <summary>Staff creates a school-owned item.</summary>
        [HttpPost("school")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> CreateSchoolItem([FromBody] ItemCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _itemService.CreateItemAsync(request, GetUserId(), fromSchool: true);
                return CreatedAtAction(nameof(GetItem), new { id = newId }, new { itemId = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── POST /api/items/personal ─────────────────────────────────────────
        /// <summary>Parent creates a personal item to share/sell.</summary>
        [HttpPost("personal")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> CreatePersonalItem([FromBody] ItemCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _itemService.CreateItemAsync(request, GetUserId(), fromSchool: false);
                return CreatedAtAction(nameof(GetItem), new { id = newId }, new { itemId = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/items/{id} ────────────────────────────────────────────
        [HttpPatch("{id}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _itemService.UpdateItemAsync(id, request);
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

        // ─── DELETE /api/items/{id} ───────────────────────────────────────────
        [HttpDelete("{id}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> DeactivateItem(int id)
        {
            try
            {
                await _itemService.DeactivateItemAsync(id);
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

        // ═══════════════════════════════════════════════════════════════════════
        // IMAGES  –  /api/items/{id}/images
        // ═══════════════════════════════════════════════════════════════════════

        // ─── POST /api/items/{id}/images ──────────────────────────────────────
        [HttpPost("{id}/images")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> AddImage(int id, [FromBody] ItemImageAddRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var imageId = await _itemService.AddImageAsync(id, request);
                return CreatedAtAction(nameof(GetItem), new { id }, new { imageId });
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

        // ─── DELETE /api/items/{id}/images/{imageId} ──────────────────────────
        [HttpDelete("{id}/images/{imageId}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> RemoveImage(int id, int imageId)
        {
            try
            {
                await _itemService.RemoveImageAsync(id, imageId);
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

        // ═══════════════════════════════════════════════════════════════════════
        // VARIANTS  –  /api/items/{id}/variants
        // ═══════════════════════════════════════════════════════════════════════

        // ─── GET /api/items/{id}/variants ─────────────────────────────────────
        [HttpGet("{id}/variants")]
        [Authorize]
        public async Task<IActionResult> GetVariants(int id)
        {
            try
            {
                var result = await _itemService.GetVariantsAsync(id);

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

        // ─── POST /api/items/{id}/variants ────────────────────────────────────
        [HttpPost("{id}/variants")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> CreateVariant(int id, [FromBody] ItemVariantCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var variantId = await _itemService.CreateVariantAsync(id, request);
                return CreatedAtAction(nameof(GetVariants), new { id }, new { variantId });
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

        // ─── PATCH /api/items/{id}/variants/{variantId} ───────────────────────
        [HttpPatch("{id}/variants/{variantId}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateVariant(int id, int variantId, [FromBody] ItemVariantUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _itemService.UpdateVariantAsync(id, variantId, request);
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

        // ─── DELETE /api/items/{id}/variants/{variantId} ──────────────────────
        [HttpDelete("{id}/variants/{variantId}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> DeleteVariant(int id, int variantId)
        {
            try
            {
                await _itemService.DeleteVariantAsync(id, variantId);
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