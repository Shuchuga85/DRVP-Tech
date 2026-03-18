using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        public readonly StudentService _studentService;
        public StudentController(StudentService studentService) 
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var result = await _studentService.GetStudentsAsync();

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
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var result = await _studentService.GetStudentAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("/parent/{id}")]
        public async Task<IActionResult> GetStudentByParent(int id)
        {
            try
            {
                var result = await _studentService.GetStudentByParentAsync(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateRequest request)
        {
            try
            {
                var result = await _studentService.CreateStudentAsync(request);

                if (!result)
                    return BadRequest("Student failed to create");

                return Ok("New Student added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateStudent(int id)
        {
            try
            {
                var result = await _studentService.SetStudentStateAsync(new StudentActivationRequest { StudentId = id, IsActive = true });

                if (!result)
                    return BadRequest("Failed to activate Student");

                return Ok("Student Activated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateStudent(int id)
        {
            try
            {
                var result = await _studentService.SetStudentStateAsync(new StudentActivationRequest { StudentId = id, IsActive = false });

                if (!result)
                    return BadRequest("Failed to deactivate Student");

                return Ok("Student Deactivated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
