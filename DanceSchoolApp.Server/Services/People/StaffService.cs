using Azure.Core;
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.People
{
    public class StaffService
    {

        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<StaffListResponse>> GetStaffsAsync()
        {
            return await _context.Users
                .Include(u => u.PersonInfo)
                .Include(u => u.IdRoles)
                .Where(u => u.IdRoles.Any(r => r.RoleId == Roles.Staff))
                .Select(r => new StaffListResponse
                {
                    StaffId = r.UserId,
                    IsActive = r.IsActive,
                    PersonInfo = r.PersonInfo == null ? null : new PersonListResponse
                    {
                        PersonId = r.PersonInfo.PersonId,
                        FirstName = r.PersonInfo.FirstName,
                        LastName = r.PersonInfo.LastName
                    }
                })
                .ToListAsync();
        }

        public async Task<StaffDetailResponse> GetStaffAsync(int id)
        {
            var staff = await _context.Users
                .Include(u => u.PersonInfo)
                .Include(u => u.IdRoles)
                .FirstOrDefaultAsync(u => u.UserId == id && u.IdRoles.Any(r => r.RoleId == Roles.Staff));

            if (staff is null)
                throw new KeyNotFoundException($"Staff with id {id} was not found.");

            return new StaffDetailResponse
            {
                StaffId = staff.UserId,
                IsActive = staff.IsActive,
                PersonInfo = staff.PersonInfo == null ? null : new PersonDetailResponse
                {
                    PersonId = staff.PersonInfo.PersonId,
                    FirstName = staff.PersonInfo.FirstName,
                    LastName = staff.PersonInfo.LastName,
                    BirthDate = staff.PersonInfo.BirthDate,
                    Phone = staff.PersonInfo.Phone,
                    Address = staff.PersonInfo.Address
                }
            };
        }

    }
}
