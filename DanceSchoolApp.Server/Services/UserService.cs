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

        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var user = await _context.Users.Include(u=> u.IdRoles)
                .ToListAsync();

            var response = user.Select(r => new UserListResponse
            {
                UserId = r.UserId,
                Usarname = r.Username,
                Email = r.Email,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                IdRoles = r.IdRoles.Select(r => new RoleSummaryResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList()
            }).ToList();

            return response;
        }

        public async Task<UserDetailResponse> GetUserAsync(int id)
        {
            var user = await _context.Users
                    .Include(u => u.Coach)
                    .Include(u => u.PersonInfo)
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                throw new Exception("Failed to find user");

            var response = new UserDetailResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                PersonInfo = user.PersonInfo == null ? null : new PersonDetailResponse
                {
                    PersonId = user.PersonInfo.PersonId,
                    FirstName = user.PersonInfo.FirstName,
                    LastName = user.PersonInfo.LastName,
                    BirthDate = user.PersonInfo.BirthDate,
                    Phone = user.PersonInfo.Phone,
                    Address = user.PersonInfo.Address
                },

                IdRoles = user.IdRoles.Select(r => new RoleSummaryResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList()
            };

            return response;
        }

        public async Task<bool> CreateUserAsync(UserCreateRequest request)
        {
            var _user = await _context.Users
                    .AnyAsync(u => u.Username == request.Username);

            if (_user)
                throw new Exception("Username already exist");

            Role role = null;

            if (request.FirstRole != null)
            {
               role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == request.FirstRole);

                if (role == null)
                    throw new Exception("Role not found");
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hash,
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                PersonInfo = request.PersonInfo == null ? null : new PersonInfo
                {
                    FirstName = request.PersonInfo.FirstName,
                    LastName = request.PersonInfo.LastName,
                    BirthDate = request.PersonInfo.BirthDate,
                    Phone = request.PersonInfo.Phone,
                    Address = request.PersonInfo.Address
                }
            };

            if (role != null) 
                user.IdRoles.Add(role);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetUserStateAsync(UserActivationRequest request)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == request.UserId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, request.IsActive));

            return rowsAffected > 0;
        }






    }
}
