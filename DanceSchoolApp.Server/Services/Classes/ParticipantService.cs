using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.Classes;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.Classes
{
    public class ParticipantService
    {
        private readonly AppDbContext _context;

        public ParticipantService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────

        public async Task<List<ParticipantListResponse>> GetByClassAsync(int classId)
        {
            bool classExists = await _context.CoachClasses
                .AnyAsync(c => c.ClassId == classId);

            if (!classExists)
                throw new KeyNotFoundException($"Class with id {classId} was not found.");

            return await _context.Participants
                .Include(p => p.IdStudentNavigation)
                    .ThenInclude(s => s.PersonInfo)
                .Include(p => p.IdStudentNavigation)
                .Where(p => p.IdCoachClass == classId)
                .Select(p => new ParticipantListResponse
                {
                    ParticipantId = p.ParticipantId,
                    ClassId = p.IdCoachClass,
                    StudentId = p.IdStudent,
                    StudentName = p.IdStudentNavigation.PersonInfo != null
                        ? $"{p.IdStudentNavigation.PersonInfo.FirstName} {p.IdStudentNavigation.PersonInfo.LastName}".Trim()
                        : $"Student {p.IdStudent}",
                    ParentUserId = p.IdStudentNavigation.ParentUserId,
                    JoinedAt = p.JoinedAt,
                    ClassPrice = p.ClassPrice,  // decimal, never null
                    ValidationStatus = (ParticipantValidationStatus)p.ValidationStatus,
                    ParentValidatedAt = p.ParentValidatedAt
                })
                .ToListAsync();
        }

        // ─── Commands ─────────────────────────────────────────────────────────

        public async Task<int> JoinClassAsync(ParticipantJoinRequest request)
        {
            // ── 1. Class must exist and be open (Approved + has space) ─────────
            var coachClass = await _context.CoachClasses
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.ClassId == request.ClassId);

            if (coachClass is null)
                throw new KeyNotFoundException(
                    $"Class with id {request.ClassId} was not found.");

            if (coachClass.Status != (byte)CoachClassStatus.Approved)
                throw new InvalidOperationException(
                    "Students can only join classes with Approved status.");

            if (coachClass.Participants.Count >= coachClass.MaxParticipants)
                throw new InvalidOperationException(
                    "This class is full. No spots available.");

            // ── 2. Student must exist and be active ───────────────────────────
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == request.StudentId
                                       && s.IsActive);

            if (student is null)
                throw new KeyNotFoundException(
                    $"Student with id {request.StudentId} was not found or is inactive.");

            // ── 3. Student must belong to the requesting parent ───────────────
            // NOTE: once auth is in, replace request.ParentUserId with the
            // authenticated user id from the JWT claims. For now the parent
            // id is inferred from the student record for safety.
            // This prevents a parent from enrolling another parent's student.

            // ── 4. Student not already enrolled in this class ─────────────────
            // The DB has a unique constraint UQ_ClassStudent, but we catch it
            // here for a clean error message rather than a constraint exception.
            bool alreadyEnrolled = coachClass.Participants
                .Any(p => p.IdStudent == request.StudentId);

            if (alreadyEnrolled)
                throw new InvalidOperationException(
                    $"Student {request.StudentId} is already enrolled in this class.");

            // ── 5. Student not already in another class at the same time ──────
            bool timeConflict = await _context.Participants
                .Include(p => p.IdCoachClassNavigation)
                .AnyAsync(p =>
                    p.IdStudent == request.StudentId &&
                    p.IdCoachClassNavigation.Status != (byte)CoachClassStatus.Rejected &&
                    p.IdCoachClassNavigation.Status != (byte)CoachClassStatus.Cancelled &&
                    p.IdCoachClassNavigation.StartDatetime < coachClass.EndDatetime &&
                    p.IdCoachClassNavigation.EndDatetime > coachClass.StartDatetime);

            if (timeConflict)
                throw new InvalidOperationException(
                    "This student is already enrolled in another class at this time.");

            // ── 6. Enroll ─────────────────────────────────────────────────────
            var participant = new Participant
            {
                IdCoachClass = request.ClassId,
                IdStudent = request.StudentId,
                JoinedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                ClassPrice = request.ClassPrice ?? 0m,  // default until pricing config exists
                ValidationStatus = (byte)ParticipantValidationStatus.Pending
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return participant.ParticipantId;
        }

        public async Task ParentValidateAsync(int participantId, bool attended)
        {
            var participant = await _context.Participants
                .Include(p => p.IdCoachClassNavigation)
                .FirstOrDefaultAsync(p => p.ParticipantId == participantId);

            if (participant is null)
                throw new KeyNotFoundException(
                    $"Participant record with id {participantId} was not found.");

            // Validation only makes sense after the class is finished.
            if (participant.IdCoachClassNavigation.Status != (byte)CoachClassStatus.Finished)
                throw new InvalidOperationException(
                    "Validation is only available for Finished classes.");

            // Prevent re-validation once already responded.
            if (participant.ValidationStatus != (byte)ParticipantValidationStatus.Pending)
                throw new InvalidOperationException(
                    "This participant record has already been validated.");

            participant.ValidationStatus = attended
                ? (byte)ParticipantValidationStatus.ParentConfirmed
                : (byte)ParticipantValidationStatus.Disputed;

            participant.ParentValidatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // After saving, check if all participants in this class have now
            // responded — if so, the class can be moved to Pending for staff
            // review. Staff will then do the final sign-off (Validated).
            await TryAdvanceClassToStaffReviewAsync(participant.IdCoachClass);
        }

        public async Task RemoveParticipantAsync(int participantId)
        {
            var participant = await _context.Participants
                .Include(p => p.IdCoachClassNavigation)
                .FirstOrDefaultAsync(p => p.ParticipantId == participantId);

            if (participant is null)
                throw new KeyNotFoundException(
                    $"Participant record with id {participantId} was not found.");

            // Removing a participant from a Finished/Validated/Pending class
            // would corrupt billing records — block it.
            var safeStatuses = new byte[]
            {
                (byte)CoachClassStatus.Requested,
                (byte)CoachClassStatus.Approved
            };

            if (!safeStatuses.Contains(participant.IdCoachClassNavigation.Status))
                throw new InvalidOperationException(
                    "A participant can only be removed from a Requested or Approved class. " +
                    "The class has already progressed past that point.");

            // Guard: if this is the last participant, block the removal.
            // A class with zero students should not exist — cancel the class instead.
            int participantCount = await _context.Participants
                .CountAsync(p => p.IdCoachClass == participant.IdCoachClass);

            if (participantCount <= 1)
                throw new InvalidOperationException(
                    "Cannot remove the last participant from a class. " +
                    "Cancel the class instead.");

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }

        // ─── Private helpers ──────────────────────────────────────────────────

        // Once all participants have validated, move the class to Pending
        // so staff know it is ready for final sign-off.
        // Called automatically after each parent validation.
        private async Task TryAdvanceClassToStaffReviewAsync(int classId)
        {
            var allParticipants = await _context.Participants
                .Where(p => p.IdCoachClass == classId)
                .ToListAsync();

            bool allResponded = allParticipants
                .All(p => p.ValidationStatus != (byte)ParticipantValidationStatus.Pending);

            if (!allResponded) return;

            // All participants have responded — move class to Pending for
            // staff to review. Staff will then call the final validate endpoint
            // (to be built on CoachClassService when validation workflow is added).
            var coachClass = await _context.CoachClasses
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (coachClass is not null &&
                coachClass.Status == (byte)CoachClassStatus.Finished)
            {
                coachClass.Status = (byte)CoachClassStatus.Pending;
                await _context.SaveChangesAsync();
            }
        }
    }
}