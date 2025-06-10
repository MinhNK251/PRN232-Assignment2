using BusinessObjectsLayer.DTO;
using BusinessObjectsLayer.Entity;
using Microsoft.Extensions.Options;

namespace NguyenKhanhMinhRazorPages.Services
{
    public interface ISystemAccountService
    {
        Task<SystemAccount?> GetAccountById(short accountId);
        Task<SystemAccount?> GetAccountByEmail(string email);
        Task<List<SystemAccount>> GetAccounts();
        Task AddAccount(SystemAccount account);
        Task UpdateAccount(SystemAccount updatedAccount);
        Task RemoveAccount(short accountId);
        Task<SystemAccount?> Login(LoginDto loginDTO);
    }
}