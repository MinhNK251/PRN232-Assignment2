using Microsoft.Extensions.Options;
using NguyenKhanhMinhRazorPages.DTOs;
using NguyenKhanhMinhRazorPages.Entity;

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
        Task<AccountResponseDto?> Login(AccountRequestDto loginDTO);
    }
}