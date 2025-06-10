using BusinessObjectsLayer.DTO;
using BusinessObjectsLayer.Entity;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly HttpClient _client;

        public SystemAccountService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7085/api/SystemAccounts/"); // API base URL
        }

        public async Task AddAccount(SystemAccount account)
        {
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);
            response.EnsureSuccessStatusCode();          
        }

        public async Task<SystemAccount?> GetAccountByEmail(string email)
        {
            var response = await _client.GetAsync($"email?email={Uri.EscapeDataString(email)}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SystemAccount>();
            }
            return null;
        }

        public async Task<SystemAccount?> GetAccountById(short accountId)
        {
            var response = await _client.GetAsync($"{accountId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SystemAccount>();
            }
            return null;
        }

        public async Task<List<SystemAccount>> GetAccounts()
        {
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SystemAccount>>();
            }
            return null;
        }

        public async Task<SystemAccount?> Login(LoginDto loginDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("login", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SystemAccount>();
            }
            return null;
        }

        public async Task RemoveAccount(short accountId)
        {
            var response = await _client.DeleteAsync($"{accountId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAccount(SystemAccount updatedAccount)
        {
            var content = new StringContent(JsonSerializer.Serialize(updatedAccount), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
