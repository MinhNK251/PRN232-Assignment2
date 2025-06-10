using BusinessObjectsLayer.Entity;

namespace ServiceLayer
{
    public interface ICategoryService
    {
        Category? GetCategoryById(short categoryId);
        List<Category> GetCategories();
        List<Category> GetActiveCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void RemoveCategory(short categoryId);
    }
}
