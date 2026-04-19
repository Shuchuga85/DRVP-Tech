using DanceSchoolApp.Server.Data;

namespace DanceSchoolApp.Server.Services.People
{
    public class PersonService
    {
        private readonly AppDbContext _context;

        public PersonService(AppDbContext context)
        {
            _context = context;
        }
    }
}
