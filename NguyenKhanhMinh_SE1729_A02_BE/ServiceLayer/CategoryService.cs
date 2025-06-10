using BusinessObjectsLayer.Entity;
using RepositoriesLayer;

namespace ServiceLayer
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _repo;

        public CategoryService(ICategoryRepo repo)
        {
            _repo = repo;
        }

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _repo.AddCategory(category);
        }

        public List<Category> GetActiveCategories()
        {
            return _repo.GetCategories().Where(c => c.IsActive == true).ToList();
        }

        public List<Category> GetCategories()
        {
            return _repo.GetCategories();
        }

        public Category? GetCategoryById(short categoryId)
        {
            return _repo.GetCategoryById(categoryId);
        }

        public void RemoveCategory(short categoryId)
        {
            var category = _repo.GetCategoryById(categoryId);

            if (category == null)
                throw new InvalidOperationException("Category not found.");

            if (category.NewsArticles != null && category.NewsArticles.Any())
                throw new InvalidOperationException("Cannot delete category that is used in news articles.");

            _repo.RemoveCategory(categoryId);
        }

        public void UpdateCategory(Category category)
        {
            _repo.UpdateCategory(category);
        }
    }
}
