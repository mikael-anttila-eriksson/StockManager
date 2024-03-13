using Stock_Manager.Models;

namespace Stock_Manager.ViewModel
{
    /// <summary>
    /// Summary of a users accounts.
    /// </summary>
    public class UserAccountsViewModel
    {
        // User data
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        
        // ------ Summary data of Accounts ---------------------------------

        /// <summary>
        /// Value of all accounts belonging to the user.
        /// </summary>
        public double TotalValue { get; set; }
        /// <summary>
        /// List of users all accounts.
        /// </summary>
        public List<AccountsViewModel> UserAccounts { get; set; }
        /// <summary>
        /// Total saldo, all accounts.
        /// </summary>
        public double TotalSaldo { get; set; }
        /// <summary>
        /// Total balance, all accounts.
        /// </summary>
        public double TotalBalance { get; set; } = 1337;
    }
}
