using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace DanceSchoolApp.Server.Services.People
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, EmailService emailService, ILogger<UserService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.IdRoles)
                .Select(u => new UserListResponse
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    Roles = u.IdRoles.Select(r => new RoleSummaryResponse
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<UserDetailResponse> GetUserAsync(int id)
        {
            var user = await _context.Users
                    .Include(u => u.Coach)
                    .Include(u => u.PersonInfo)
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == id);

            if (user is null)
                throw new KeyNotFoundException($"User with id {id} was not found.");

            return new UserDetailResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                PersonInfo = user.PersonInfo is null ? null : new PersonDetailResponse
                {
                    PersonId = user.PersonInfo.PersonId,
                    FirstName = user.PersonInfo.FirstName,
                    LastName = user.PersonInfo.LastName,
                    BirthDate = user.PersonInfo.BirthDate,
                    Phone = user.PersonInfo.Phone,
                    Address = user.PersonInfo.Address,
                    Nif = user.PersonInfo.Nif
                },
                Roles = user.IdRoles.Select(r => new RoleSummaryResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList()
            };
        }

        public async Task<UserRolesResponse> GetUserRolesAsync(int id)
        {
            var user = await _context.Users
                    .Include(u => u.Coach)
                    .Include(u => u.IdRoles)
                    .FirstOrDefaultAsync(u => u.UserId == id);

            if (user is null)
                throw new KeyNotFoundException($"User with id {id} was not found.");

            return new UserRolesResponse
            {
                UserId = user.UserId,
                Roles = user.IdRoles.Select(r => new RoleSummaryResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList()
            };
        }

        // ─── Commands ─────────────────────────────────────────────────────────

        public async Task<int> CreateUserAsync(UserCreateRequest request)
        {
            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username == request.Username);

            if (usernameExists)
                throw new InvalidOperationException("Username already exists.");

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email);

            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            Role? role = null;

            if (request.FirstRole is not null) 
            {
                role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.RoleId == request.FirstRole);

                if (role is null)
                    throw new KeyNotFoundException($"Role with id {request.FirstRole} was not found.");
            }

            string generatedPassword = Guid.NewGuid().ToString("N")[..8];

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword),
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                PersonInfo = request.PersonInfo is null ? null : new PersonInfo
                {
                    FirstName = request.PersonInfo.FirstName,
                    LastName = request.PersonInfo.LastName,
                    BirthDate = request.PersonInfo.BirthDate,
                    Phone = request.PersonInfo.Phone,
                    Address = request.PersonInfo.Address,
                    Nif = request.PersonInfo.Nif

                }
            };

            if (role is not null)
                user.IdRoles.Add(role);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            try
            {
                string roleName = role?.RoleName ?? "Utilizador";
                await _emailService.SendWelcomeEmailAsync(user.Email, roleName, generatedPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "O utilizador {Email} foi criado com ID {Id}, mas o email de boas-vindas falhou. Pass: {Pass}", user.Email, user.UserId, generatedPassword);
            }

            return user.UserId;
        }

        public async Task SetUserStateAsync(int userId, bool isActive)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.UserId == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, isActive));

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"User with id {userId} was not found.");
        }



    }
}
