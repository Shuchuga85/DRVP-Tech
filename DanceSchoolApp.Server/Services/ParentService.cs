using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class ParentService
    {

        public readonly AppDbContext _context;

        public ParentService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<ParentListResponse>> GetParentsAsync()
        {
            var users = await _context.Users
                .Include(u => u.PersonInfo)
                .Include(u => u.IdRoles)
                .Where(u => u.IdRoles.Any(r => r.RoleId == Roles.Parent))
                .ToListAsync();

            var response = users.Select(r => new ParentListResponse
            {
                ParentId = r.UserId,
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

        public async Task<ParentDetailResponse> GetParentAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.PersonInfo)
                .Include(u => u.IdRoles)
                .FirstOrDefaultAsync(u => u.UserId == id && u.IdRoles.Any(r => r.RoleId == Roles.Parent));

            if (user == null)
                throw new Exception("Failed to find parent");

            var response = new ParentDetailResponse
            {
                ParentId = user.UserId,
                IsActive = user.IsActive,
                PersonInfo = user.PersonInfo == null ? null : new PersonDetailResponse
                {
                    PersonId = user.PersonInfo.PersonId,
                    FirstName = user.PersonInfo.FirstName,
                    LastName = user.PersonInfo.LastName,
                    BirthDate = user.PersonInfo.BirthDate,
                    Phone = user.PersonInfo.Phone,
                    Address = user.PersonInfo.Address
                }
            };

            return response;
        }

        /*
        public async Task<bool> CreateParentAsync(ParentCreateRequest request)
        {
            var user = await _context.Users
                    .Include(u => u.Parent)
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Parent != null)
                throw new Exception("User already has parent profile");

            var parent = new Parent
            {
                ParentId = user.UserId,
                ParentNavigation = user,
                PersonInfo = new PersonInfo
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Phone = request.Phone,
                    Address = request.Address
                }
            };

            _context.Parents.Add(parent);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetParentStateAsync(ParentActivationRequest request)
        {
            var rowsAffected = await _context.Parents
                .Where(u => u.ParentId == request.ParentId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsActive, request.IsActive));

            return rowsAffected > 0;
        }
        */


    }
}
