using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.Inventory;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services.Inventory
{
    public class ItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        // ─── Item Queries ─────────────────────────────────────────────────────────

        public async Task<List<ItemListResponse>> GetItemsAsync(bool? fromSchool = null)
        {
            var query = _context.Items
                .Include(i => i.IdCategoryNavigation)
                .Include(i => i.ItemImages)
                .Where(i => i.IsActive);

            if (fromSchool.HasValue)
                query = query.Where(i => i.FromSchool == fromSchool.Value);

            return await query.Select(i => new ItemListResponse
            {
                ItemId = i.ItemId,
                Name = i.Name,
                Description = i.Description,
                FromSchool = i.FromSchool,
                IdOwner = i.IdOwner,
                IsActive = i.IsActive,
                CreatedAt = i.CreatedAt,
                Category = i.IdCategoryNavigation == null ? null : new ItemCategorySummaryResponse
                {
                    CategoryId = i.IdCategoryNavigation.CategoryId,
                    CatgName = i.IdCategoryNavigation.CatgName
                },
                Images = i.ItemImages.Select(img => new ItemImageResponse
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList()
            }).ToListAsync();
        }

        public async Task<ItemDetailResponse> GetItemAsync(int id)
        {
            var item = await _context.Items
                .Include(i => i.IdCategoryNavigation)
                .Include(i => i.IdContactNavigation)
                .Include(i => i.ItemImages)
                .Include(i => i.ItemVariants)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item is null)
                throw new KeyNotFoundException($"Item with id {id} was not found.");

            return MapToDetail(item);
        }

        // ─── Item Commands ────────────────────────────────────────────────────────

        public async Task<int> CreateItemAsync(ItemCreateRequest request, int ownerUserId, bool fromSchool)
        {
            ItemContact? contact = null;

            if (request.Contact is not null)
            {
                contact = new ItemContact
                {
                    PhoneNumber = request.Contact.PhoneNumber,
                    Email = request.Contact.Email,
                    Address = request.Contact.Address
                };
                _context.ItemContacts.Add(contact);
                await _context.SaveChangesAsync();
            }

            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                FromSchool = fromSchool,
                IdOwner = ownerUserId,
                IdCategory = request.IdCategory,
                IdContact = contact?.IcontactId,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = true
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return item.ItemId;
        }

        public async Task UpdateItemAsync(int id, ItemUpdateRequest request)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);

            if (item is null)
                throw new KeyNotFoundException($"Item with id {id} was not found.");

            if (request.Name is not null) item.Name = request.Name;
            if (request.Description is not null) item.Description = request.Description;
            if (request.IdCategory.HasValue) item.IdCategory = request.IdCategory;

            await _context.SaveChangesAsync();
        }

        public async Task DeactivateItemAsync(int id)
        {
            var rows = await _context.Items
                .Where(i => i.ItemId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, false));

            if (rows == 0)
                throw new KeyNotFoundException($"Item with id {id} was not found.");
        }

        // ─── Contact ──────────────────────────────────────────────────────────────

        public async Task UpsertContactAsync(int itemId, ItemContactUpsertRequest request)
        {
            var item = await _context.Items
                .Include(i => i.IdContactNavigation)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);

            if (item is null)
                throw new KeyNotFoundException($"Item with id {itemId} was not found.");

            if (item.IdContactNavigation is not null)
            {
                item.IdContactNavigation.PhoneNumber = request.PhoneNumber;
                item.IdContactNavigation.Email = request.Email;
                item.IdContactNavigation.Address = request.Address;
            }
            else
            {
                var contact = new ItemContact
                {
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Address = request.Address
                };
                _context.ItemContacts.Add(contact);
                await _context.SaveChangesAsync();
                item.IdContact = contact.IcontactId;
            }

            await _context.SaveChangesAsync();
        }

        // ─── Images ───────────────────────────────────────────────────────────────

        public async Task<int> AddImageAsync(int itemId, ItemImageAddRequest request)
        {
            var exists = await _context.Items.AnyAsync(i => i.ItemId == itemId);
            if (!exists)
                throw new KeyNotFoundException($"Item with id {itemId} was not found.");

            var image = new ItemImage
            {
                IdItem = itemId,
                ImageUrl = request.ImageUrl
            };

            _context.ItemImages.Add(image);
            await _context.SaveChangesAsync();

            return image.ImageId;
        }

        public async Task RemoveImageAsync(int itemId, int imageId)
        {
            var image = await _context.ItemImages
                .FirstOrDefaultAsync(img => img.ImageId == imageId && img.IdItem == itemId);

            if (image is null)
                throw new KeyNotFoundException($"Image with id {imageId} not found on item {itemId}.");

            _context.ItemImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        // ─── Variants ─────────────────────────────────────────────────────────────

        public async Task<List<ItemVariantDetailResponse>> GetVariantsAsync(int itemId)
        {
            var exists = await _context.Items.AnyAsync(i => i.ItemId == itemId);
            if (!exists)
                throw new KeyNotFoundException($"Item with id {itemId} was not found.");

            return await _context.ItemVariants
                .Where(v => v.IdItem == itemId)
                .Select(v => new ItemVariantDetailResponse
                {
                    VariantId = v.VariantId,
                    IdItem = v.IdItem,
                    Color = v.Color,
                    Size = v.Size,
                    Quantity = v.Quantity,
                    Price = v.Price,
                    IsActive = v.IsActive
                }).ToListAsync();
        }

        public async Task<int> CreateVariantAsync(int itemId, ItemVariantCreateRequest request)
        {
            var exists = await _context.Items.AnyAsync(i => i.ItemId == itemId);
            if (!exists)
                throw new KeyNotFoundException($"Item with id {itemId} was not found.");

            var variant = new ItemVariant
            {
                IdItem = itemId,
                Color = request.Color,
                Size = request.Size,
                Quantity = request.Quantity,
                Price = request.Price,
                IsActive = true
            };

            _context.ItemVariants.Add(variant);
            await _context.SaveChangesAsync();

            return variant.VariantId;
        }

        public async Task UpdateVariantAsync(int itemId, int variantId, ItemVariantUpdateRequest request)
        {
            var variant = await _context.ItemVariants
                .FirstOrDefaultAsync(v => v.VariantId == variantId && v.IdItem == itemId);

            if (variant is null)
                throw new KeyNotFoundException($"Variant with id {variantId} not found on item {itemId}.");

            if (request.Color is not null) variant.Color = request.Color;
            if (request.Size is not null) variant.Size = request.Size;
            if (request.Quantity.HasValue) variant.Quantity = request.Quantity.Value;
            if (request.Price.HasValue) variant.Price = request.Price;
            if (request.IsActive.HasValue) variant.IsActive = request.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteVariantAsync(int itemId, int variantId)
        {
            var variant = await _context.ItemVariants
                .FirstOrDefaultAsync(v => v.VariantId == variantId && v.IdItem == itemId);

            if (variant is null)
                throw new KeyNotFoundException($"Variant with id {variantId} not found on item {itemId}.");

            var hasActiveRequisitions = await _context.ItemRequisitions
                .AnyAsync(r => r.ItemVariantId == variantId && r.Status == 1);

            if (hasActiveRequisitions)
                throw new InvalidOperationException("Cannot delete a variant with active requisitions.");

            _context.ItemVariants.Remove(variant);
            await _context.SaveChangesAsync();
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private static ItemDetailResponse MapToDetail(Item item) => new()
        {
            ItemId = item.ItemId,
            Name = item.Name,
            Description = item.Description,
            FromSchool = item.FromSchool,
            IdOwner = item.IdOwner,
            IsActive = item.IsActive,
            CreatedAt = item.CreatedAt,
            Category = item.IdCategoryNavigation == null ? null : new ItemCategorySummaryResponse
            {
                CategoryId = item.IdCategoryNavigation.CategoryId,
                CatgName = item.IdCategoryNavigation.CatgName
            },
            Contact = item.IdContactNavigation == null ? null : new ItemContactResponse
            {
                IcontactId = item.IdContactNavigation.IcontactId,
                PhoneNumber = item.IdContactNavigation.PhoneNumber,
                Email = item.IdContactNavigation.Email,
                Address = item.IdContactNavigation.Address
            },
            Images = item.ItemImages.Select(img => new ItemImageResponse
            {
                ImageId = img.ImageId,
                ImageUrl = img.ImageUrl
            }).ToList(),
            Variants = item.ItemVariants.Select(v => new ItemVariantSummaryResponse
            {
                VariantId = v.VariantId,
                Color = v.Color,
                Size = v.Size,
                Quantity = v.Quantity,
                Price = v.Price,
                IsActive = v.IsActive
            }).ToList()
        };
    }
}