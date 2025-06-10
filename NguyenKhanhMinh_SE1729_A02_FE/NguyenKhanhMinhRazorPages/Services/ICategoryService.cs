using BusinessObjectsLayer.Entity;

namespace NguyenKhanhMinhRazorPages.Services
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryById(short categoryId);
        Task<List<Category>> GetCategories();
        Task<List<Category>> GetActiveCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task RemoveCategory(short categoryId);
    }
}
