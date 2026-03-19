using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace DanceSchoolApp.Server.Services.People
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<StudentListResponse>> GetStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.PersonInfo)
                .Select(s => new StudentListResponse
                {
                    StudentId = s.StudentId,
                    ParentUserId = s.ParentUserId,
                    IsActive = s.IsActive,
                    PersonInfo = s.PersonInfo == null ? null : new PersonListResponse
                    {
                        PersonId = s.PersonInfo.PersonId,
                        FirstName = s.PersonInfo.FirstName,
                        LastName = s.PersonInfo.LastName
                    }
                })
                .ToListAsync();
        }

        public async Task<StudentDetailResponse> GetStudentAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.PersonInfo)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student is null)
                throw new KeyNotFoundException($"Student with id {id} was not found.");

            return new StudentDetailResponse
            {
                StudentId = student.StudentId,
                ParentUserId = student.ParentUserId,
                IsActive = student.IsActive,
                PersonInfo = student.PersonInfo is null ? null : new PersonDetailResponse
                {
                    PersonId = student.PersonInfo.PersonId,
                    FirstName = student.PersonInfo.FirstName,
                    LastName = student.PersonInfo.LastName,
                    BirthDate = student.PersonInfo.BirthDate,
                    Phone = student.PersonInfo.Phone,
                    Address = student.PersonInfo.Address
                }
            };
        }

        public async Task<List<StudentListResponse>> GetStudentsByParentAsync(int parentId)
        {
            // Verify the parent user exists first so we can return a
            // meaningful 404 rather than just an empty list.
            bool parentExists = await _context.Users
                .AnyAsync(u => u.UserId == parentId);

            if (!parentExists)
                throw new KeyNotFoundException($"User with id {parentId} was not found.");

            return await _context.Students
                .Include(s => s.PersonInfo)
                .Where(s => s.ParentUserId == parentId)
                .Select(s => new StudentListResponse
                {
                    StudentId = s.StudentId,
                    ParentUserId = s.ParentUserId,
                    IsActive = s.IsActive,
                    PersonInfo = s.PersonInfo == null ? null : new PersonListResponse
                    {
                        PersonId = s.PersonInfo.PersonId,
                        FirstName = s.PersonInfo.FirstName,
                        LastName = s.PersonInfo.LastName
                    }
                })
                .ToListAsync();
        }

        // ─── Commands ─────────────────────────────────────────────────────────

        public async Task<int> CreateStudentAsync(StudentCreateRequest request)
        {
            bool parentExists = await _context.Users
                .AnyAsync(u => u.UserId == request.ParentId);

            if (!parentExists)
                throw new KeyNotFoundException($"User with id {request.ParentId} was not found.");

            var student = new Student
            {
                ParentUserId = request.ParentId,
                IsActive = true,
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

            return student.StudentId;
        }

        public async Task UpdateStudentAsync(int id, StudentUpdateRequest request)
        {
            var student = await _context.Students
                .Include(s => s.PersonInfo)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student is null)
                throw new KeyNotFoundException($"Student with id {id} was not found.");

            // PersonInfo is always created alongside the student, but guard
            // defensively in case of data inconsistency.
            if (student.PersonInfo is null)
                throw new InvalidOperationException($"Student with id {id} has no personal info record.");

            student.PersonInfo.FirstName = request.FirstName;
            student.PersonInfo.LastName = request.LastName;
            student.PersonInfo.BirthDate = request.BirthDate;
            student.PersonInfo.Phone = request.Phone;
            student.PersonInfo.Address = request.Address;

            await _context.SaveChangesAsync();
        }

        public async Task SetStudentStateAsync(int studentId, bool isActive)
        {
            var rowsAffected = await _context.Students
                .Where(s => s.StudentId == studentId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, isActive));

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"Student with id {studentId} was not found.");
        }

    }
}
