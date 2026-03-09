using Azure.Core;
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class StaffService
    {

        public readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<StaffListResponse>> GetStaffsAsync()
        {
            var staff = await _context.Staff.Include(p => p.PersonInfo).ToListAsync();

            var response = staff.Select(r => new StaffListResponse
            {
                StaffId = r.StaffId,
                Position = r.Position,
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

        public async Task<StaffDetailResponse> GetStaffAsync(int id)
        {
            var staff = await _context.Staff.Include(p => p.PersonInfo)
                    .FirstOrDefaultAsync(p => p.StaffId == id);

            if (staff == null)
                throw new Exception("Failed to find Staff");

            var response = new StaffDetailResponse
            {
                StaffId = staff.StaffId,
                Position = staff.Position,
                IsActive = staff.IsActive,
                PersonInfo = new PersonDetailResponse
                {
                    PersonId = staff.PersonInfo.PersonId,
                    FirstName = staff.PersonInfo.FirstName,
                    LastName = staff.PersonInfo.LastName,
                    BirthDate = staff.PersonInfo.BirthDate,
                    Email = staff.PersonInfo.Email,
                    Phone = staff.PersonInfo.Phone,
                    Address = staff.PersonInfo.Address
                }
            };

            return response;
        }

        public async Task<bool> CreateStaffAsync(StaffCreateRequest request)
        {
            var user = await _context.Users
                    .Include(u => u.IdRoles)
                    .Include(u => u.Staff)
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Staff != null)
                throw new Exception("User already has Staff profile");

            var Staff = new Staff
            {
                StaffId = user.UserId,
                Position = request.Position,
                StaffNavigation = user,
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

            _context.Staff.Add(Staff);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetStaffStateAsync(StaffActivationRequest request)
        {
            var rowsAffected = await _context.Staff
                .Where(u => u.StaffId == request.StaffId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, request.IsActive));

            return rowsAffected > 0;
        }

    }
}
