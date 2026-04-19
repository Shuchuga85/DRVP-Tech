using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.DTOs.Admin;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class AdminPortalService
    {
        private readonly AppDbContext _context;

        public AdminPortalService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Dashboard ────────────────────────────────────────────────────────

        public async Task<AdminDashboardResponse> GetDashboardAsync()
        {
            var totalStaff = await _context.Users
                .CountAsync(u => u.IdRoles.Any(r => r.RoleId == Roles.Staff));

            var activeStaff = await _context.Users
                .CountAsync(u => u.IsActive && u.IdRoles.Any(r => r.RoleId == Roles.Staff));

            return new AdminDashboardResponse
            {
                TotalStaffAccounts  = totalStaff,
                ActiveStaffAccounts = activeStaff
            };
        }

        // ─── Staff users list ─────────────────────────────────────────────────

        public async Task<PagedResult<AdminUserRow>> GetStaffUsersAsync(
            string? search, int page, int pageSize)
        {
            var baseQuery = _context.Users
                .Include(u => u.PersonInfo)
                .Where(u => u.IdRoles.Any(r => r.RoleId == Roles.Staff));

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                baseQuery = baseQuery.Where(u =>
                    (u.PersonInfo != null &&
                     (u.PersonInfo.FirstName + " " + u.PersonInfo.LastName)
                         .ToLower().Contains(term)) ||
                    u.Username.ToLower().Contains(term) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)));
            }

            var total = await baseQuery.CountAsync();

            var users = await baseQuery
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = users.Select(u => new AdminUserRow
            {
                UserId   = u.UserId,
                Name     = u.PersonInfo is not null
                    ? $"{u.PersonInfo.FirstName} {u.PersonInfo.LastName}".Trim()
                    : u.Username,
                Email    = u.Email,
                IsActive = u.IsActive
            }).ToList();

            return new PagedResult<AdminUserRow>
            {
                Items      = items,
                TotalCount = total,
                Page       = page,
                PageSize   = pageSize
            };
        }
    }
}
