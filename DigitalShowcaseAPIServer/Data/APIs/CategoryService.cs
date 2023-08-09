using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class CategoryService : ICategoryService
    {
        private readonly DigitalShowcaseContext _db;

        public CategoryService(DigitalShowcaseContext db)
        {
            _db = db;
        }

        public async Task<Category?> AddCategoryAsync(Category item)
        {
            if (item is null) return null;
            await _db.Categories.AddAsync(item);
            await _db.SaveChangesAsync();

            return item;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(Category.CategoryId id)
        {
            return await _db.Categories.SingleAsync(cat => cat.Id == id);
        }

        public async Task<Category?> SaveCategoryAsync(Category item)
        {
            var category = await _db.Categories.FindAsync(item.Id);

            if (category is null) return null;

            category.Name = item.Name;
            category.Priority = item.Priority;
            category.IsVisible = item.IsVisible;
            category.IconURL = item.IconURL;

            await _db.SaveChangesAsync();

            return category;
        }
    }
}
