
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace DanceSchoolApp.Server.Services
{
    public class StudentService
    {
        public readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<StudentListResponse>> GetStudentsAsync()
        {
            var students = await _context.Students.Include(s => s.PersonInfo).ToListAsync();

            var response = students.Select(r => new StudentListResponse
            {
                StudentId = r.StudentId,
                IdParent = r.IdParent,
                IsActive = r.IsActive,
                PersonInfo = r.PersonInfo == null ? null : new PersonListResponse
                {
                    PersonId = r.PersonInfo.PersonId,
                    FirstName = r.PersonInfo.FirstName,
                    LastName = r.PersonInfo.LastName
                }
            }).ToList();

            return response;
        }

        public async Task<StudentDetailResponse> GetStudentAsync(int id)
        {
            var student = await _context.Students.Include(s => s.PersonInfo)
                    .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                throw new Exception("Failed to find student");

            var response = new StudentDetailResponse
            {
                StudentId = student.StudentId,
                IdParent = student.IdParent,
                IsActive = student.IsActive,
                PersonInfo = new PersonDetailResponse
                {
                    PersonId = student.PersonInfo.PersonId,
                    FirstName= student.PersonInfo.FirstName,
                    LastName= student.PersonInfo.LastName,
                    BirthDate = student.PersonInfo.BirthDate,
                    Email = student.PersonInfo.Email,
                    Phone = student.PersonInfo.Phone,
                    Address = student.PersonInfo.Address
                }
            };

            return response;
        }

        public async Task<List<StudentListResponse>> GetStudentByParentAsync(int id)
        {
            var parent = await _context.Parents
                .Include(p => p.Students)
                    .ThenInclude(s => s.PersonInfo)
                .FirstOrDefaultAsync(p => p.ParentId == id);

            if (parent == null)
                throw new Exception("Failed to find parent");

            var response = parent.Students.Select(r => new StudentListResponse
            {
                StudentId = r.StudentId,
                IdParent = r.IdParent,
                IsActive = r.IsActive,
                PersonInfo = r.PersonInfo == null ? null : new PersonListResponse
                {
                    PersonId = r.PersonInfo.PersonId,
                    FirstName = r.PersonInfo.FirstName,
                    LastName = r.PersonInfo.LastName
                }
            }).ToList();

            return response;
        }

        public async Task<bool> CreateStudentAsync(StudentCreateRequest request)
        {
            var parent = await _context.Parents
                    .Include(u => u.Students)
                    .FirstOrDefaultAsync(u => u.ParentId == request.ParentId);

            if (parent == null)
                throw new Exception("Parent not found");

            var student = new Student
            {
                IdParent = parent.ParentId,
                IdParentNavigation = parent,
                PersonInfo = new PersonInfo
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address
                }
            };

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetStudentStateAsync(StudentActivationRequest request)
        {
            var rowsAffected = await _context.Students
                .Where(u => u.StudentId == request.StudentId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, request.IsActive));

            return rowsAffected > 0;
        }

    }
}
