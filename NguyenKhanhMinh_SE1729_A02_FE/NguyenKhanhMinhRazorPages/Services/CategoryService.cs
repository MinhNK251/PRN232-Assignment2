﻿using NguyenKhanhMinhRazorPages.Entity;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _client.BaseAddress = new Uri("https://localhost:7085/api/Categories/"); // API base URL
        }

        private void AddJwtHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }


        public async Task AddCategory(Category category)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Category>> GetActiveCategories()
        {
            AddJwtHeader();
            var response = await _client.GetAsync("active");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            return null;
        }

        public async Task<List<Category>> GetCategories()
        {
            AddJwtHeader();
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            return null;
        }

        public async Task<Category?> GetCategoryById(short categoryId)
        {
            AddJwtHeader();
            var response = await _client.GetAsync($"{categoryId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Category>();
            }
            return null;
        }

        public async Task RemoveCategory(short categoryId)
        {
            AddJwtHeader();
            var response = await _client.DeleteAsync($"{categoryId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategory(Category category)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
