using NguyenKhanhMinhRazorPages.DTOs;
using NguyenKhanhMinhRazorPages.Entity;
using System.Text;
using System.Text.Json;

namespace NguyenKhanhMinhRazorPages.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SystemAccountService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _client.BaseAddress = new Uri("https://localhost:7085/api/SystemAccounts/");
        }

        private void AddJwtHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task AddAccount(SystemAccount account)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("", content);
            response.EnsureSuccessStatusCode();          
        }

        public async Task<SystemAccount?> GetAccountByEmail(string email)
        {
            AddJwtHeader();
            var response = await _client.GetAsync($"email?email={Uri.EscapeDataString(email)}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SystemAccount>();
            }
            return null;
        }

        public async Task<SystemAccount?> GetAccountById(short accountId)
        {
            AddJwtHeader();
            var response = await _client.GetAsync($"{accountId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SystemAccount>();
            }
            return null;
        }

        public async Task<List<SystemAccount>> GetAccounts()
        {
            AddJwtHeader();
            var response = await _client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SystemAccount>>();
            }
            return null;
        }

        public async Task<AccountResponseDto?> Login(AccountRequestDto loginDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("login", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AccountResponseDto>();
            }
            return null;
        }

        public async Task RemoveAccount(short accountId)
        {
            AddJwtHeader();
            var response = await _client.DeleteAsync($"{accountId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAccount(SystemAccount updatedAccount)
        {
            AddJwtHeader();
            var content = new StringContent(JsonSerializer.Serialize(updatedAccount), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
