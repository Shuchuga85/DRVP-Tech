using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.DTOs.People;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace DanceSchoolApp.Server.Services.People
{
    public class CoachService
    {
        private readonly AppDbContext _context;

        public CoachService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<CoachListResponse>> GetCoachsAsync()
        {
            return await _context.Users
                .Include(u => u.PersonInfo)
                .Include(u => u.IdRoles)
                .Include(u => u.Coach)
                .Where(u => u.IdRoles.Any(r => r.RoleId == Roles.Coach))
                .Select(r => new CoachListResponse
                {
                    CoachId = r.UserId,
                    Biography = r.Coach == null ? null : r.Coach.Biography,
                    PhotoUrl = r.Coach == null ? null : r.Coach.PhotoUrl,
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

        public async Task<CoachDetailResponse> GetCoachAsync(int id)
        {
            var coach = await _context.Users
               .Include(u => u.PersonInfo)
               .Include(u => u.IdRoles)
               .Include(u => u.Coach)
               .FirstOrDefaultAsync(u => u.UserId == id && u.IdRoles.Any(r => r.RoleId == Roles.Coach));


            if (coach is null)
                throw new KeyNotFoundException($"Coach with id {id} was not found.");

            return new CoachDetailResponse
            {
                CoachId = coach.UserId,
                Biography = coach.Coach == null ? null : coach.Coach.Biography,
                PhotoUrl = coach.Coach == null ? null : coach.Coach.PhotoUrl,
                IsActive = coach.IsActive,
                PersonInfo = coach.PersonInfo is null ? null : new PersonDetailResponse
                {
                    PersonId = coach.PersonInfo.PersonId,
                    FirstName = coach.PersonInfo.FirstName,
                    LastName = coach.PersonInfo.LastName,
                    BirthDate = coach.PersonInfo.BirthDate,
                    Phone = coach.PersonInfo.Phone,
                    Address = coach.PersonInfo.Address
                }
            };
        }

    }
}
