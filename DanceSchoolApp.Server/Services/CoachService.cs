using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace DanceSchoolApp.Server.Services
{
    public class CoachService
    {
        public readonly AppDbContext _context;

        public CoachService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<CoachListResponse>> GetCoachsAsync()
        {
            var coaches = await _context.Coaches.Include(p => p.PersonInfo).ToListAsync();

            var response = coaches.Select(r => new CoachListResponse
            {
                CoachId = r.CoachId,
                Biography = r.Biography,
                PhotoUrl = r.PhotoUrl,
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

        public async Task<CoachDetailResponse> GetCoachAsync(int id)
        {
            var coach = await _context.Coaches.Include(p => p.PersonInfo)
                    .FirstOrDefaultAsync(p => p.CoachId == id);

            if (coach == null)
                throw new Exception("Failed to find Coach");

            var response = new CoachDetailResponse
            {
                CoachId = coach.CoachId,
                Biography = coach.Biography,
                PhotoUrl = coach.PhotoUrl,
                IsActive = coach.IsActive,
                PersonInfo = new PersonDetailResponse
                {
                    PersonId = coach.PersonInfo.PersonId,
                    FirstName = coach.PersonInfo.FirstName,
                    LastName = coach.PersonInfo.LastName,
                    BirthDate = coach.PersonInfo.BirthDate,
                    Email = coach.PersonInfo.Email,
                    Phone = coach.PersonInfo.Phone,
                    Address = coach.PersonInfo.Address
                }
            };

            return response;
        }

        public async Task<bool> CreateCoachAsync(CoachCreateRequest request)
        {
            var user = await _context.Users
                    .Include(u => u.IdRoles)
                    .Include(u => u.Coach)
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Coach != null)
                throw new Exception("User already has Coach profile");

            var Coach = new Coach
            {
                CoachId = user.UserId,
                Biography = request.Biography,
                PhotoUrl = request.PhotoUrl,
                CoachNavigation = user,
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

            _context.Coaches.Add(Coach);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetCoachStateAsync(CoachActivationRequest request)
        {
            var rowsAffected = await _context.Coaches
                .Where(u => u.CoachId == request.CoachId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, request.IsActive));

            return rowsAffected > 0;
        }
    }
}
