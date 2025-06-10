using BusinessObjectsLayer.DTO;
using BusinessObjectsLayer.Entity;
using Microsoft.Identity.Client;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly HttpClient _client;

        public NewsArticleService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7085/api/NewsArticles/"); // API base URL
        }

        public async Task AddNewsArticle(NewsArticleDto dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Error Response: " + error);
                throw new HttpRequestException($"API returned {response.StatusCode}: {error}");
            }
        }

        public async Task<List<NewsArticle>> GetActiveNewsArticles()
        {
            var response = await _client.GetAsync("active");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<NewsArticle>>();
            }
            return null;
        }

        public async Task<List<NewsArticle>> GetArticlesByTagId(int tagId)
        {
            var response = await _client.GetAsync($"by-tag/{tagId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<NewsArticle>>();
            }
            return null;
        }

        public async Task<NewsArticle?> GetNewsArticleById(string articleId)
        {
            var response = await _client.GetAsync($"{articleId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<NewsArticle>();
            }
            return null;
        }

        public async Task<List<NewsArticle>> GetNewsArticles()
        {
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<NewsArticle>>();
            }
            return null;
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByCreatedBy(int createdById)
        {
            var response = await _client.GetAsync($"created-by/{createdById}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<NewsArticle>>();
            }
            return null;
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByDateRange(DateRangeDto dateRangeDto)
        {
            var query = $"by-date-range?StartDate={dateRangeDto.StartDate:O}&EndDate={dateRangeDto.EndDate:O}";
            var response = await _client.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<NewsArticle>>();
            }
            return new List<NewsArticle>();
        }

        public async Task RemoveNewsArticle(string articleId)
        {
            var response = await _client.DeleteAsync($"{articleId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveTagsByArticleId(string articleId)
        {
            var response = await _client.DeleteAsync($"{articleId}/tags");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateNewsArticle(NewsArticleDto updatedArticle)
        {
            var content = new StringContent(JsonSerializer.Serialize(updatedArticle), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{updatedArticle.NewsArticleId}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
