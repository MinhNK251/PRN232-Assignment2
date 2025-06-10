using BusinessObjectsLayer.Entity;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _client;

        public CategoryService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7085/api/Categories/"); // API base URL
        }

        public async Task AddCategory(Category category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Category>> GetActiveCategories()
        {
            var response = await _client.GetAsync("active");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            return null;
        }

        public async Task<List<Category>> GetCategories()
        {
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            return null;
        }

        public async Task<Category?> GetCategoryById(short categoryId)
        {
            var response = await _client.GetAsync($"{categoryId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Category>();
            }
            return null;
        }

        public async Task RemoveCategory(short categoryId)
        {
            var response = await _client.DeleteAsync($"{categoryId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategory(Category category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
