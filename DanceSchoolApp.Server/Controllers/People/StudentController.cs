using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Services.People;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DanceSchoolApp.Server.Controllers.People
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentService _studentService;
        public StudentController(StudentService studentService) 
        {
            _studentService = studentService;
        }

        // ─── GET /api/students ─────────────────────────────────────────────────
        [Authorize(Roles = "staff")]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var result = await _studentService.GetStudentsAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── GET /api/students/{id} ────────────────────────────────────────────
        [Authorize(Roles = "staff,parent")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var result = await _studentService.GetStudentAsync(id);

                if (!IsStaff() && result.ParentUserId != GetUserId())
                    return Forbid();

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

        // ─── GET /api/students/parent/{parentId} ───────────────────────────────
        [Authorize(Roles = "staff,parent")]
        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetStudentsByParent(int parentId)
        {
            if (!IsStaff() && parentId != GetUserId())
                return Forbid();

            try
            {
                var result = await _studentService.GetStudentsByParentAsync(parentId);

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

        // ─── Helpers ──────────────────────────────────────────────────────────
        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsStaff() => User.IsInRole("staff");

        // ─── POST /api/students ────────────────────────────────────────────────
        [Authorize(Roles = "staff,parent")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _studentService.CreateStudentAsync(request);
                return CreatedAtAction(nameof(GetStudent), new { id = newId }, new { studentId = newId });
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

        // ─── PATCH /api/students/{id} ──────────────────────────────────────────
        [Authorize(Roles = "staff,parent")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _studentService.UpdateStudentAsync(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ─── PATCH /api/students/{id}/activate ────────────────────────────────
        [Authorize(Roles = "staff")]
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateStudent(int id)
        {
            try
            {
                await _studentService.SetStudentStateAsync(id, isActive: true);
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

        // ─── PATCH /api/students/{id}/deactivate ──────────────────────────────
        [Authorize(Roles = "staff")]
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateStudent(int id)
        {
            try
            {
                await _studentService.SetStudentStateAsync(id, isActive: false);
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
