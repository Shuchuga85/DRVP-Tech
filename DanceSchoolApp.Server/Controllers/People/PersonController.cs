using DanceSchoolApp.Server.Services.People;
using Microsoft.AspNetCore.Mvc;

namespace DanceSchoolApp.Server.Controllers.People
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
        // Placeholder — methods moved to dedicated controllers.
    }
}
