using Azure.Core;
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class UserService
    {
        public readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponse>> AllUsersAsync()
        {
            var user = await _context.Users.ToListAsync();

            var response = user.Select(r => new UserResponse
            {
                UserId = r.UserId,
                Usarname = r.Username,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt
            }).ToList();

            return response;
        }

        public async Task<UserAltResponse> GetUserAsync(int id)
        {
            var user = await _context.Users
                    .Include(u => u.Coach)
                    .Include(u => u.Parent)
                    .Include(u => u.Staff)
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                throw new Exception("Failed to find user");

            var response = new UserAltResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,

                Coach = user.Coach == null ? null : new CoachResponse
                {
                    CoachId = user.Coach.CoachId
                },

                Parent = user.Parent == null ? null : new ParentResponse
                {
                    ParentId = user.Parent.ParentId
                },

                Staff = user.Staff == null ? null : new StaffResponse
                {
                    StaffId = user.Staff.StaffId,
                    Position = user.Staff.Position
                },

                IdRoles = user.IdRoles.Select(r => new RoleAltResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList()
            };

            return response;
        }

        public async Task<bool> SetUserActiveAsync(UserActiveRequest request)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == request.UserId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, request.IsActive));

            return rowsAffected > 0;
        }






    }
}
