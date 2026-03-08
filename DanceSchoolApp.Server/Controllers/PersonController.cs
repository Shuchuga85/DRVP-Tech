using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }


        #region Add Requests

        [HttpPost("add/staff")]
        public async Task<IActionResult> AddStaff([FromBody] StaffRequest request)
        {
            try
            {
                var result = await _personService.AddStaffAsync(request);

                if (!result)
                    return BadRequest("Staff failed to create");

                return Ok("New Staff added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add/coach")]
        public async Task<IActionResult> AddCoach([FromBody] CoachRequest request)
        {
            try
            {
                var result = await _personService.AddCoachAsync(request);

                if (!result)
                    return BadRequest("Coach failed to create");

                return Ok("New Coach added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add/parent")]
        public async Task<IActionResult> AddParent([FromBody] ParentRequest request)
        {
            try
            {
                var result = await _personService.AddParentAsync(request);

                if (!result)
                    return BadRequest("Parent failed to create");

                return Ok("New Parent added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add/student")]
        public async Task<IActionResult> AddStudent([FromBody] StudentRequest request)
        {
            try
            {
                var result = await _personService.AddStudentAsync(request);

                if (!result)
                    return BadRequest("Student failed to create");

                return Ok("New Student added");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion add



    }
}
