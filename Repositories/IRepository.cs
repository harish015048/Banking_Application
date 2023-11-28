using Banking_Application.Models;

namespace Banking_Application.Repositories
{
    public interface IRepository
    {
        Task<List<UserModel>> GetUsersAsync();
        Task<UserModel> GetUserAsync(int userId);
        Task<UserModel> AddUserAsync(string userName);
        Task<bool> DeleteUserAsync(int userId);

        Task<AccountModel> AddAccountAsync(int userId, decimal initialBalance);
        Task<bool> DeleteAccountAsync(int userId, int accountId);

        Task<bool> DepositAsync(int accountId, decimal amount);
        Task<bool> WithdrawAsync(int accountId, decimal amount);
    }
}
