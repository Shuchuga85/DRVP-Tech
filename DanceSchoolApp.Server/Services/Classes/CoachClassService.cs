using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.Classes;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.Classes
{
    public class CoachClassService
    {
        private readonly AppDbContext _context;

        public CoachClassService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<CoachClassListResponse>> GetAllAsync()
        {
            return await _context.CoachClasses
                .Include(c => c.IdModalityNavigation)
                .Include(c => c.IdStudioNavigation)
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.CoachNavigation)
                        .ThenInclude(u => u.PersonInfo)
                .Include(c => c.Participants)
                .Select(c => MapToListResponse(c))
                .ToListAsync();
        }

        public async Task<CoachClassDetailResponse> GetByIdAsync(int id)
        {
            var coachClass = await _context.CoachClasses
                .Include(c => c.IdModalityNavigation)
                .Include(c => c.IdStudioNavigation)
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.CoachNavigation)
                        .ThenInclude(u => u.PersonInfo)
                .Include(c => c.Participants)
                    .ThenInclude(p => p.IdStudentNavigation)
                        .ThenInclude(s => s.PersonInfo)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (coachClass is null)
                throw new KeyNotFoundException($"Class with id {id} was not found.");

            return new CoachClassDetailResponse
            {
                ClassId = coachClass.ClassId,
                Status = (CoachClassStatus)coachClass.Status,
                StartDatetime = coachClass.StartDatetime,
                EndDatetime = coachClass.EndDatetime,
                ModalityId = coachClass.IdModality,
                ModalityName = coachClass.IdModalityNavigation.Name,
                StudioId = coachClass.IdStudio,
                StudioName = coachClass.IdStudioNavigation.Name,
                CoachId = coachClass.IdCoach,
                CoachName = ResolveCoachName(coachClass.IdCoachNavigation),
                CreatedByUserId = coachClass.CreatedBy,
                MaxParticipants = coachClass.MaxParticipants,
                CurrentParticipants = coachClass.Participants.Count,
                CreatedAt = coachClass.CreatedAt,
                CoachValidatedAt = coachClass.CoachValidatedAt,
                StaffValidatedAt = coachClass.StaffValidatedAt,
                Participants = coachClass.Participants.Select(p => new ClassParticipantSummary
                {
                    ParticipantId = p.ParticipantId,
                    StudentId = p.IdStudent,
                    StudentName = ResolveStudentName(p.IdStudentNavigation),
                    JoinedAt = p.JoinedAt,
                    ValidationStatus = p.ValidationStatus
                }).ToList()
            };
        }

        public async Task<List<OpenClassResponse>> GetOpenClassesAsync()
        {
            // Open = Approved status with available spots.
            // Materialise first because EF cannot translate the Count comparison
            // on a navigation collection to SQL in all versions.
            var classes = await _context.CoachClasses
                .Include(c => c.IdModalityNavigation)
                .Include(c => c.IdStudioNavigation)
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.CoachNavigation)
                        .ThenInclude(u => u.PersonInfo)
                .Include(c => c.Participants)
                .Where(c => c.Status == (byte)CoachClassStatus.Approved)
                .ToListAsync();

            return classes
                .Where(c => c.Participants.Count < c.MaxParticipants)
                .Select(c => new OpenClassResponse
                {
                    ClassId = c.ClassId,
                    StartDatetime = c.StartDatetime,
                    EndDatetime = c.EndDatetime,
                    ModalityName = c.IdModalityNavigation.Name,
                    StudioName = c.IdStudioNavigation.Name,
                    CoachName = ResolveCoachName(c.IdCoachNavigation),
                    MaxParticipants = c.MaxParticipants,
                    SpotsAvailable = c.MaxParticipants - c.Participants.Count
                })
                .ToList();
        }

        public async Task<List<CoachClassListResponse>> GetByStatusAsync(CoachClassStatus status)
        {
            return await _context.CoachClasses
                .Include(c => c.IdModalityNavigation)
                .Include(c => c.IdStudioNavigation)
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.CoachNavigation)
                        .ThenInclude(u => u.PersonInfo)
                .Include(c => c.Participants)
                .Where(c => c.Status == (byte)status)
                .Select(c => MapToListResponse(c))
                .ToListAsync();
        }

        public async Task<List<CoachClassListResponse>> GetByParentAsync(int parentUserId)
        {
            bool parentExists = await _context.Users
                .AnyAsync(u => u.UserId == parentUserId);

            if (!parentExists)
                throw new KeyNotFoundException($"User with id {parentUserId} was not found.");

            // Return classes where any enrolled student belongs to this parent.
            var classes = await _context.CoachClasses
                .Include(c => c.IdModalityNavigation)
                .Include(c => c.IdStudioNavigation)
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.CoachNavigation)
                        .ThenInclude(u => u.PersonInfo)
                .Include(c => c.Participants)
                    .ThenInclude(p => p.IdStudentNavigation)
                .Where(c => c.Participants
                    .Any(p => p.IdStudentNavigation.ParentUserId == parentUserId))
                .ToListAsync();

            return classes.Select(MapToListResponse).ToList();
        }

        // ─── Commands ─────────────────────────────────────────────────────────

        public async Task<int> CreateAsync(CoachClassCreateRequest request, int createdByUserId)
        {
            // ── 1. Validate all referenced entities exist ──────────────────────
            bool modalityActive = await _context.Modalities
                .AnyAsync(m => m.ModalityId == request.ModalityId && m.IsActive);

            if (!modalityActive)
                throw new KeyNotFoundException(
                    $"Modality with id {request.ModalityId} was not found or is inactive.");

            bool studioActive = await _context.Studios
                .AnyAsync(s => s.StudioId == request.StudioId && s.IsActive);

            if (!studioActive)
                throw new KeyNotFoundException(
                    $"Studio with id {request.StudioId} was not found or is inactive.");

            bool coachActive = await _context.Coaches
                .AnyAsync(c => c.CoachId == request.CoachId);

            if (!coachActive)
                throw new KeyNotFoundException(
                    $"Coach with id {request.CoachId} was not found.");

            // ── 2. Studio supports the requested modality ──────────────────────
            bool compatibleStudio = await _context.Studios
                .Where(s => s.StudioId == request.StudioId)
                .AnyAsync(s => s.IdModalities
                    .Any(m => m.ModalityId == request.ModalityId));

            if (!compatibleStudio)
                throw new InvalidOperationException(
                    $"Studio {request.StudioId} does not support modality {request.ModalityId}.");

            // ── 3. Blocked period check ────────────────────────────────────────
            await CheckBlockedPeriodsAsync(
                request.StartDatetime, request.EndDatetime,
                request.CoachId, request.StudioId);

            // ── 4. Studio not double-booked ────────────────────────────────────
            bool studioConflict = await _context.CoachClasses
                .AnyAsync(c =>
                    c.IdStudio == request.StudioId &&
                    c.Status != (byte)CoachClassStatus.Rejected &&
                    c.Status != (byte)CoachClassStatus.Cancelled &&
                    c.StartDatetime < request.EndDatetime &&
                    c.EndDatetime > request.StartDatetime);

            if (studioConflict)
                throw new InvalidOperationException(
                    "The studio is already booked during this time window.");

            // ── 5. Coach not double-booked ─────────────────────────────────────
            bool coachConflict = await _context.CoachClasses
                .AnyAsync(c =>
                    c.IdCoach == request.CoachId &&
                    c.Status != (byte)CoachClassStatus.Rejected &&
                    c.Status != (byte)CoachClassStatus.Cancelled &&
                    c.StartDatetime < request.EndDatetime &&
                    c.EndDatetime > request.StartDatetime);

            if (coachConflict)
                throw new InvalidOperationException(
                    "The coach is already booked during this time window.");

            // ── 6. Validate all student ids belong to this parent ──────────────
            var parentStudentIds = await _context.Students
                .Where(s => s.ParentUserId == createdByUserId && s.IsActive)
                .Select(s => s.StudentId)
                .ToListAsync();

            var invalidStudents = request.StudentIds
                .Except(parentStudentIds)
                .ToList();

            if (invalidStudents.Any())
                throw new InvalidOperationException(
                    $"Student id(s) {string.Join(", ", invalidStudents)} do not belong " +
                    $"to parent {createdByUserId} or are inactive.");

            // ── 7. Create class + participants atomically ──────────────────────
            var coachClass = new CoachClass
            {
                IdModality = request.ModalityId,
                IdStudio = request.StudioId,
                IdCoach = request.CoachId,
                CreatedBy = createdByUserId,
                StartDatetime = request.StartDatetime,
                EndDatetime = request.EndDatetime,
                MaxParticipants = request.MaxParticipants,
                Status = (byte)CoachClassStatus.Requested,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _context.CoachClasses.Add(coachClass);

            // Save first to get ClassId, then add participants.
            // Both operations are inside the same EF change tracker so if
            // SaveChangesAsync fails the whole thing rolls back.
            await _context.SaveChangesAsync();

            foreach (var studentId in request.StudentIds)
            {
                _context.Participants.Add(new Participant
                {
                    IdCoachClass = coachClass.ClassId,
                    IdStudent = studentId,
                    JoinedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    ValidationStatus = 0  // pending validation
                });
            }

            await _context.SaveChangesAsync();

            return coachClass.ClassId;
        }

        // ─── Status transitions ───────────────────────────────────────────────

        public async Task ApproveAsync(int classId)
        {
            await TransitionStatusAsync(
                classId,
                allowedFrom: new[] { CoachClassStatus.Requested },
                newStatus: CoachClassStatus.Approved,
                errorMessage: "Only a Requested class can be approved."
            );
        }

        public async Task RejectAsync(int classId, string? reason)
        {
            await TransitionStatusAsync(
                classId,
                allowedFrom: new[] { CoachClassStatus.Requested },
                newStatus: CoachClassStatus.Rejected,
                errorMessage: "Only a Requested class can be rejected."
            );
            // TODO: persist reason to a notification for the parent
            // once NotificationService is built.
        }

        public async Task CancelAsync(int classId)
        {
            await TransitionStatusAsync(
                classId,
                allowedFrom: new[] { CoachClassStatus.Requested, CoachClassStatus.Approved },
                newStatus: CoachClassStatus.Cancelled,
                errorMessage: "Only a Requested or Approved class can be cancelled."
            );
        }

        public async Task FinishAsync(int classId)
        {
            await TransitionStatusAsync(
                classId,
                allowedFrom: new[] { CoachClassStatus.Approved },
                newStatus: CoachClassStatus.Finished,
                errorMessage: "Only an Approved class can be marked as finished."
            );
        }

        // ─── Private helpers ──────────────────────────────────────────────────

        private async Task CheckBlockedPeriodsAsync(
            DateTime start, DateTime end, int coachId, int studioId)
        {
            // Any global block (Undefined=0, NormalClass=1, Event=4, Holiday=5)
            // that overlaps the requested window blocks all bookings.
            bool globalBlock = await _context.BlockedPeriods
                .AnyAsync(b =>
                    (b.Scope == 0 || b.Scope == 1 || b.Scope == 4 || b.Scope == 5) &&
                    b.StartDatetime < end &&
                    b.EndDatetime > start);

            if (globalBlock)
                throw new InvalidOperationException(
                    "The requested time window falls within a global blocked period " +
                    "(school event, holiday, or normal class block).");

            // Studio-specific block
            bool studioBlock = await _context.BlockedPeriods
                .AnyAsync(b =>
                    b.Scope == 2 &&
                    b.IdStudio == studioId &&
                    b.StartDatetime < end &&
                    b.EndDatetime > start);

            if (studioBlock)
                throw new InvalidOperationException(
                    "The studio is blocked during this time window.");

            // Coach-specific block
            bool coachBlock = await _context.BlockedPeriods
                .AnyAsync(b =>
                    b.Scope == 3 &&
                    b.IdCoach == coachId &&
                    b.StartDatetime < end &&
                    b.EndDatetime > start);

            if (coachBlock)
                throw new InvalidOperationException(
                    "The coach is unavailable during this time window.");
        }

        // Generic status transition — enforces allowed-from states so invalid
        // transitions (e.g. approving an already-validated class) are caught
        // in one place rather than duplicated per method.
        private async Task TransitionStatusAsync(
            int classId,
            CoachClassStatus[] allowedFrom,
            CoachClassStatus newStatus,
            string errorMessage)
        {
            var coachClass = await _context.CoachClasses
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (coachClass is null)
                throw new KeyNotFoundException($"Class with id {classId} was not found.");

            var currentStatus = (CoachClassStatus)coachClass.Status;

            if (!allowedFrom.Contains(currentStatus))
                throw new InvalidOperationException(errorMessage);

            coachClass.Status = (byte)newStatus;
            await _context.SaveChangesAsync();
        }

        private static CoachClassListResponse MapToListResponse(CoachClass c) =>
            new CoachClassListResponse
            {
                ClassId = c.ClassId,
                Status = (CoachClassStatus)c.Status,
                StartDatetime = c.StartDatetime,
                EndDatetime = c.EndDatetime,
                ModalityName = c.IdModalityNavigation.Name,
                StudioName = c.IdStudioNavigation.Name,
                CoachName = ResolveCoachName(c.IdCoachNavigation),
                MaxParticipants = c.MaxParticipants,
                CurrentParticipants = c.Participants.Count,
                CreatedAt = c.CreatedAt
            };

        private static string ResolveCoachName(Coach coach)
        {
            var person = coach.CoachNavigation?.PersonInfo;
            return person is not null
                ? $"{person.FirstName} {person.LastName}".Trim()
                : coach.CoachNavigation?.Username ?? $"Coach {coach.CoachId}";
        }

        private static string ResolveStudentName(Student student)
        {
            var person = student.PersonInfo;
            return person is not null
                ? $"{person.FirstName} {person.LastName}".Trim()
                : $"Student {student.StudentId}";
        }
    }
}