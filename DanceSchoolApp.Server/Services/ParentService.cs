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
            var parents = await _context.Parents.Include(p => p.PersonInfo).ToListAsync();

            var response = parents.Select(r => new ParentListResponse
            {
                ParentId = r.ParentId,
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
            var parent = await _context.Parents.Include(p => p.PersonInfo)
                    .FirstOrDefaultAsync(p => p.ParentId == id);

            if (parent == null)
                throw new Exception("Failed to find parent");

            var response = new ParentDetailResponse
            {
                ParentId = parent.ParentId,
                IsActive = parent.IsActive,
                PersonInfo = new PersonDetailResponse
                {
                    PersonId = parent.PersonInfo.PersonId,
                    FirstName = parent.PersonInfo.FirstName,
                    LastName = parent.PersonInfo.LastName,
                    BirthDate = parent.PersonInfo.BirthDate,
                    Email = parent.PersonInfo.Email,
                    Phone = parent.PersonInfo.Phone,
                    Address = parent.PersonInfo.Address
                }
            };

            return response;
        }

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
                    Email = request.Email,
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



    }
}
