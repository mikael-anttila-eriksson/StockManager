using Stock_Manager.Models;
using System.ComponentModel.DataAnnotations;

namespace Stock_Manager.ViewModel
{
    public class AccountsViewModel
    {
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        public List<AccountLineViewModel> VMAccountLines { get; set; }

        [Display(Name = "User:")]
        public int UserId { get; set; }
        public AppUser? User { get; set; }
        /// <summary>
        /// Optional name for account.
        /// </summary>
        [Display(Name = "Account name")]
        public string? Name { get; set; }
        public int NumOfStocks { get; set; }
        [Display(Name = "Total value")]
        public double ValueAccount { get; set; }
        public double Saldo { get; set; }
        /// <summary>
        /// Balance of Account
        /// </summary>
        public double? Balance { get; set; }
    }
}
