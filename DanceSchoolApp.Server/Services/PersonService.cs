using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class PersonService
    {
        private readonly AppDbContext _context;

        public PersonService(AppDbContext context)
        {
            _context = context;
        }

        /*
        public async Task<bool> AddStaffAsync(StaffRequest request)
        {
            var user = await _context.Users
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Staff != null)
                throw new Exception("User already has staff profile");

            var staff = new Staff
            {
                StaffId = user.UserId,
                Position = request.Position,
                PersonInfo = new PersonInfo
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Phone = request.Phone,
                    Address = request.Address
                }
            };

            _context.Staff.Add(staff);

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddCoachAsync(CoachRequest request)
        {
            var user = await _context.Users
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Coach != null)
                throw new Exception("User already has coach profile");

            var coach = new Coach
            {
                CoachId = user.UserId,
                Biography = request.Biography,
                PhotoUrl = request.PhotoUrl,
                PersonInfo = new PersonInfo
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Phone = request.Phone,
                    Address = request.Address
                }
            };

            _context.Coaches.Add(coach);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AddParentAsync(ParentRequest request)
        {
            var user = await _context.Users
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Coach != null)
                throw new Exception("User already has parent profile");

            var parent = new Parent
            {
                ParentId = user.UserId,
                PersonInfo = new PersonInfo
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Phone = request.Phone,
                    Address = request.Address
                }
            };

            _context.Parents.Add(parent);

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddStudentAsync(StudentRequest request)
        {
            var parent = await _context.Parents
                    .Include(u => u.Students)
                    .FirstOrDefaultAsync(u => u.ParentId == request.ParentId);

            if (parent == null)
                throw new Exception("Parent not found");

            var student = new Student
            {
                IdParentNavigation = parent,
                PersonInfo = new PersonInfo
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Phone = request.Phone,
                    Address = request.Address
                }
            };

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return true;
        }
        */
    }
}
