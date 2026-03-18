using Azure.Core;
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class RoleService
    {
        public readonly AppDbContext _context;
        public RoleService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateRoleAsync(RoleCreateRequest request)
        {
            if (request.RoleName == null)
                throw new Exception("rolename is null");
            var role = new Role
            {
                RoleId = request.RoleId,
                RoleName = request.RoleName
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<RoleResponse> GetRoleAsync(int id)
        {
            var role = await _context.Roles
                   .Include(r => r.IdUsers) 
                   .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null) throw new Exception("Failed to find Role");

            var response = new RoleResponse
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Users = role.IdUsers
                    .Select(u => new UserListResponse
                    {
                        UserId = u.UserId,
                        Usarname = u.Username,
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt,
                        IdRoles = null
                       
                    }).ToList()
                 };
            return response;
        }

        public async Task<bool> AddRoleAsync(RoleAssign request)
        {
            var user = await _context.Users
                   .Include(u => u.IdRoles)
                   .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == request.RoleId);

            if (role == null)
                throw new Exception("Role not found");

            if (user.IdRoles.Any(r => r.RoleId == request.RoleId))
                throw new Exception("User already has this role");

            user.IdRoles.Add(role);

            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> RemoveRoleAsync(RoleAssign request)
        {
            var user = await _context.Users
                .Include(u => u.IdRoles)
                .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            var role = user.IdRoles.FirstOrDefault(r => r.RoleId == request.RoleId);

            if (role == null)
                throw new Exception("Role not assigned");

            user.IdRoles.Remove(role);

            await _context.SaveChangesAsync();

            return true;
        }


    }

}
