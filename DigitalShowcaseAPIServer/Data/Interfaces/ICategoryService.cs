using DigitalShowcaseAPIServer.Data.Models;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync();
        public Task<Category?> GetCategoryAsync(Category.CategoryId id);
    }
}
