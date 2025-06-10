using BusinessObjectsLayer.Entity;
using DAOsLayer;
using Microsoft.Extensions.Options;

namespace RepositoriesLayer
{
    public class SystemAccountRepo : ISystemAccountRepo
    {
        public SystemAccount? GetAccountById(short accountId)
            => SystemAccountDAO.Instance.GetAccountById(accountId);

        public SystemAccount? GetAccountByEmail(string email)
            => SystemAccountDAO.Instance.GetAccountByEmail(email);

        public List<SystemAccount> GetAccounts()
            => SystemAccountDAO.Instance.GetAccounts();

        public void AddAccount(SystemAccount account)
            => SystemAccountDAO.Instance.AddAccount(account);

        public void UpdateAccount(SystemAccount updatedAccount)
            => SystemAccountDAO.Instance.UpdateAccount(updatedAccount);

        public void RemoveAccount(short accountId)
            => SystemAccountDAO.Instance.RemoveAccount(accountId);

        public SystemAccount? Login(string email, string password, IOptions<AdminAccountSettings> adminAccountSettings)
            => SystemAccountDAO.Instance.Login(email, password, adminAccountSettings);
    }
}
