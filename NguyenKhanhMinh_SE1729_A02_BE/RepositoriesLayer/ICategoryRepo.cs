using BusinessObjectsLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoriesLayer
{
    public interface ICategoryRepo
    {
        Category? GetCategoryById(short categoryId);
        List<Category> GetCategories();
        List<Category> GetActiveCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void RemoveCategory(short categoryId);
    }
}