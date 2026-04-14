using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.Inventory;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.Inventory
{
    // Requisition status constants
    public static class RequisitionStatus
    {
        public const byte Pending = 0;
        public const byte Approved = 1;
        public const byte Rejected = 2;
        public const byte Returned = 3;
    }

    public class ItemRequisitionService
    {
        private readonly AppDbContext _context;

        public ItemRequisitionService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Queries ──────────────────────────────────────────────────────────────

        /// <summary>Returns all requisitions (staff view).</summary>
        public async Task<List<ItemRequisitionListResponse>> GetAllAsync()
        {
            return await BuildQuery().ToListAsync();
        }

        /// <summary>Returns only the requisitions that belong to a given parent user.</summary>
        public async Task<List<ItemRequisitionListResponse>> GetByParentAsync(int parentUserId)
        {
            return await BuildQuery()
                .Where(r => r.IdParent == parentUserId)
                .ToListAsync();
        }

        public async Task<ItemRequisitionDetailResponse> GetByIdAsync(int id)
        {
            var req = await _context.ItemRequisitions
                .Include(r => r.ItemVariant)
                    .ThenInclude(v => v.IdItemNavigation)
                .FirstOrDefaultAsync(r => r.RequisitionId == id);

            if (req is null)
                throw new KeyNotFoundException($"Requisition with id {id} was not found.");

            return MapToDetail(req);
        }

        // ─── Commands ─────────────────────────────────────────────────────────────

        public async Task<int> CreateAsync(ItemRequisitionCreateRequest request, int parentUserId)
        {
            var variant = await _context.ItemVariants
                .FirstOrDefaultAsync(v => v.VariantId == request.ItemVariantId && v.IsActive == true);

            if (variant is null)
                throw new KeyNotFoundException($"Variant with id {request.ItemVariantId} was not found or is inactive.");

            if (variant.Quantity < request.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock. Available: {variant.Quantity}, requested: {request.Quantity}.");

            var requisition = new ItemRequisition
            {
                ItemVariantId = request.ItemVariantId,
                IdParent = parentUserId,
                Quantity = request.Quantity,
                RequestedAt = DateTime.UtcNow,
                ExpectedReturnDate = request.ExpectedReturnDate,
                Note = request.Note,
                Status = RequisitionStatus.Pending
            };

            _context.ItemRequisitions.Add(requisition);
            await _context.SaveChangesAsync();

            return requisition.RequisitionId;
        }

        /// <summary>Staff approves or rejects a pending requisition.</summary>
        public async Task ReviewAsync(int id, ItemRequisitionReviewRequest request)
        {
            var requisition = await _context.ItemRequisitions
                .Include(r => r.ItemVariant)
                .FirstOrDefaultAsync(r => r.RequisitionId == id);

            if (requisition is null)
                throw new KeyNotFoundException($"Requisition with id {id} was not found.");

            if (requisition.Status != RequisitionStatus.Pending)
                throw new InvalidOperationException("Only pending requisitions can be reviewed.");

            if (request.Approve)
            {
                // Deduct stock
                if (requisition.ItemVariant.Quantity < requisition.Quantity)
                    throw new InvalidOperationException("Insufficient stock at time of approval.");

                requisition.ItemVariant.Quantity -= requisition.Quantity;
                requisition.Status = RequisitionStatus.Approved;
                requisition.NeedFrom = DateTime.UtcNow;
            }
            else
            {
                requisition.Status = RequisitionStatus.Rejected;
                requisition.NeedUntil = DateTime.UtcNow;
            }

            if (request.Note is not null)
                requisition.Note = request.Note;

            await _context.SaveChangesAsync();
        }

        /// <summary>Parent registers the return of borrowed items.</summary>
        public async Task ReturnAsync(int id, ItemRequisitionReturnRequest request)
        {
            var requisition = await _context.ItemRequisitions
                .Include(r => r.ItemVariant)
                .FirstOrDefaultAsync(r => r.RequisitionId == id);

            if (requisition is null)
                throw new KeyNotFoundException($"Requisition with id {id} was not found.");

            if (requisition.Status != RequisitionStatus.Approved)
                throw new InvalidOperationException("Only approved requisitions can be returned.");

            if (request.ReturnQuantity > requisition.Quantity)
                throw new InvalidOperationException(
                    $"Return quantity ({request.ReturnQuantity}) exceeds requisition quantity ({requisition.Quantity}).");

            // Restore stock
            requisition.ItemVariant.Quantity += request.ReturnQuantity;
            requisition.ReturnQuantity = request.ReturnQuantity;
            requisition.ReturnedAt = DateTime.UtcNow;
            requisition.Status = RequisitionStatus.Returned;
            if (request.ReturnNote is not null) requisition.Note = request.ReturnNote;

            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(int id, int requestingUserId, bool isStaff)
        {
            var requisition = await _context.ItemRequisitions
                .FirstOrDefaultAsync(r => r.RequisitionId == id);

            if (requisition is null)
                throw new KeyNotFoundException($"Requisition with id {id} was not found.");

            if (!isStaff && requisition.IdParent != requestingUserId)
                throw new UnauthorizedAccessException("You do not have permission to cancel this requisition.");

            if (requisition.Status != RequisitionStatus.Pending)
                throw new InvalidOperationException("Only pending requisitions can be cancelled.");

            _context.ItemRequisitions.Remove(requisition);
            await _context.SaveChangesAsync();
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private IQueryable<ItemRequisitionListResponse> BuildQuery()
        {
            return _context.ItemRequisitions
                .Include(r => r.ItemVariant)
                    .ThenInclude(v => v.IdItemNavigation)
                .Select(r => new ItemRequisitionListResponse
                {
                    RequisitionId = r.RequisitionId,
                    ItemVariantId = r.ItemVariantId,
                    ItemName = r.ItemVariant.IdItemNavigation.Name,
                    VariantColor = r.ItemVariant.Color,
                    VariantSize = r.ItemVariant.Size,
                    IdParent = r.IdParent,
                    Quantity = r.Quantity,
                    RequestedAt = r.RequestedAt,
                    ApprovedAt = r.NeedFrom,
                    RejectedAt = r.NeedUntil,
                    ExpectedReturnDate = r.ExpectedReturnDate,
                    ReturnedAt = r.ReturnedAt,
                    Status = r.Status,
                    Note = r.Note
                });
        }

        private static ItemRequisitionDetailResponse MapToDetail(ItemRequisition r) => new()
        {
            RequisitionId = r.RequisitionId,
            ItemVariantId = r.ItemVariantId,
            ItemName = r.ItemVariant.IdItemNavigation.Name,
            VariantColor = r.ItemVariant.Color,
            VariantSize = r.ItemVariant.Size,
            IdParent = r.IdParent,
            Quantity = r.Quantity,
            RequestedAt = r.RequestedAt,
            ApprovedAt = r.NeedFrom,
            RejectedAt = r.NeedUntil,
            ExpectedReturnDate = r.ExpectedReturnDate,
            ReturnedAt = r.ReturnedAt,
            Status = r.Status,
            Note = r.Note,
            ReturnQuantity = r.ReturnQuantity
        };
    }
}