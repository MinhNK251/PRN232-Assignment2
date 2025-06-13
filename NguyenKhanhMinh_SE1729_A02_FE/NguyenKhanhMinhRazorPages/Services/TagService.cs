using Microsoft.Identity.Client;
using NguyenKhanhMinhRazorPages.Entity;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TagService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _client.BaseAddress = new Uri("https://localhost:7085/api/Tags/"); // API base URL
        }

        private void AddJwtHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task AddTag(Tag tag)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Tag> GetTagById(int id)
        {
            AddJwtHeader();
            var response = await _client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Tag>();
            }
            return null;
        }

        public async Task<List<Tag>> GetTags()
        {
            AddJwtHeader();
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            return null;
        }

        public async Task<List<Tag>> GetTagsByIds(List<int> tagIds)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(tagIds), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("by-ids", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            return null;
        }

        public async Task<List<Tag>> GetTagsByNewsArticleId(string newsArticleId)
        {
            AddJwtHeader();
            var response = await _client.GetAsync($"article/{newsArticleId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            return null;
        }

        public async Task RemoveArticlesByTagId(int tagId)
        {
            AddJwtHeader();
            var response = await _client.DeleteAsync($"{tagId}/articles");
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveTag(int id)
        {
            AddJwtHeader();
            var response = await _client.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTag(Tag tag)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
