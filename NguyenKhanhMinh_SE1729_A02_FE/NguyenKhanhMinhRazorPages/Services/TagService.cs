using BusinessObjectsLayer.DTO;
using BusinessObjectsLayer.Entity;
using Microsoft.Identity.Client;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _client;

        public TagService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7085/api/Tags/"); // API base URL
        }

        public async Task AddTag(Tag tag)
        {
            var content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Tag> GetTagById(int id)
        {
            var response = await _client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Tag>();
            }
            return null;
        }

        public async Task<List<Tag>> GetTags()
        {
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            return null;
        }

        public async Task<List<Tag>> GetTagsByIds(List<int> tagIds)
        {
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
            var response = await _client.GetAsync($"article/{newsArticleId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            return null;
        }

        public async Task RemoveArticlesByTagId(int tagId)
        {
            var response = await _client.DeleteAsync($"{tagId}/articles");
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveTag(int id)
        {
            var response = await _client.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTag(Tag tag)
        {
            var content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
