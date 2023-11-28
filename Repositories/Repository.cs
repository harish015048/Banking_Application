using Banking_Application.Models;

namespace Banking_Application.Repositories
{
    public class Repository:IRepository
    {
        private static List<UserModel> users = new List<UserModel>();
        private static int userIdCounter = 1;
        private static int accountIdCounter = 1;

        public async Task<List<UserModel>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            return await Task.FromResult(users.FirstOrDefault(u => u.UserId == userId));
        }

        public async Task<UserModel> AddUserAsync(string userName)
        {
            var newUser = new UserModel
            {
                UserId = userIdCounter++,
                UserName = userName
            };

            users.Add(newUser);

            return await Task.FromResult(newUser);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var userToRemove = users.FirstOrDefault(u => u.UserId == userId);

            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                return true;
            }

            return await Task.FromResult(false);
        }

        public async Task<AccountModel> AddAccountAsync(int userId, decimal initialBalance)
        {
            if (initialBalance < 100)
            {
                throw new ArgumentException("Initial balance must be at least $100.");
            }

            var newAccount = new AccountModel
            {
                AccountId = accountIdCounter++,
                Balance = initialBalance
            };

            var user = users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                user.Accounts.Add(newAccount);
            }

            return await Task.FromResult(newAccount);
        }

        public async Task<bool> DeleteAccountAsync(int userId, int accountId)
        {
            var user = users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var accountToRemove = user.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (accountToRemove != null)
                {
                    user.Accounts.Remove(accountToRemove);
                    return true;
                }
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DepositAsync(int accountId, decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Deposit amount cannot be negative.");
            }

            var account = users.SelectMany(u => u.Accounts).FirstOrDefault(a => a.AccountId == accountId);

            if (account != null && amount <= 10000)
            {
                account.Balance += amount;
                return true;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> WithdrawAsync(int accountId, decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Withdrawal amount cannot be negative.");
            }

            var account = users.SelectMany(u => u.Accounts).FirstOrDefault(a => a.AccountId == accountId);

            if (account != null && amount <= account.Balance * 0.9m && account.Balance - amount >= 100)
            {
                account.Balance -= amount;
                return true;
            }

            return await Task.FromResult(false);
        }
    }
}
