namespace Banking_Application.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<AccountModel> Accounts { get; set; } = new List<AccountModel>();
    }
}
