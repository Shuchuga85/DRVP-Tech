using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.Social;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.Social
{
    public class NewsPostService
    {
        private readonly AppDbContext _context;

        public NewsPostService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<NewsPostListResponse>> GetAllAsync()
        {
            return await _context.NewsPosts
                .Include(p => p.CreatedByNavigation)
                    .ThenInclude(u => u!.PersonInfo)
                .Select(p => new NewsPostListResponse
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    CreatedByName = ResolveAuthorName(p.CreatedByNavigation)
                })
                .ToListAsync();
        }

        public async Task<NewsPostDetailResponse> GetByIdAsync(int id)
        {
            var post = await _context.NewsPosts
                .Include(p => p.CreatedByNavigation)
                    .ThenInclude(u => u!.PersonInfo)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post is null)
                throw new KeyNotFoundException($"News post with id {id} was not found.");

            return new NewsPostDetailResponse
            {
                PostId = post.PostId,
                Title = post.Title,
                Subtitle = post.Subtitle,
                Description = post.Description,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt,
                CreatedByUserId = post.CreatedBy,
                CreatedByName = ResolveAuthorName(post.CreatedByNavigation)
            };
        }

        // ─── Commands ─────────────────────────────────────────────────────────

        public async Task<int> CreateAsync(NewsPostCreateRequest request)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.UserId == request.CreatedByUserId);

            if (!userExists)
                throw new KeyNotFoundException(
                    $"User with id {request.CreatedByUserId} was not found.");

            var post = new NewsPost
            {
                Title = request.Title,
                Subtitle = request.Subtitle,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                CreatedBy = request.CreatedByUserId
            };

            _context.NewsPosts.Add(post);
            await _context.SaveChangesAsync();

            return post.PostId;
        }

        public async Task UpdateAsync(int id, NewsPostUpdateRequest request)
        {
            var post = await _context.NewsPosts
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post is null)
                throw new KeyNotFoundException($"News post with id {id} was not found.");

            post.Title = request.Title;
            post.Subtitle = request.Subtitle;
            post.Description = request.Description;
            post.ImageUrl = request.ImageUrl;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rowsAffected = await _context.NewsPosts
                .Where(p => p.PostId == id)
                .ExecuteDeleteAsync();

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"News post with id {id} was not found.");
        }

        // ─── Private helpers ──────────────────────────────────────────────────

        private static string? ResolveAuthorName(User? user)
        {
            if (user is null) return null;
            var person = user.PersonInfo;
            return person is not null
                ? $"{person.FirstName} {person.LastName}".Trim()
                : user.Username;
        }
    }
}