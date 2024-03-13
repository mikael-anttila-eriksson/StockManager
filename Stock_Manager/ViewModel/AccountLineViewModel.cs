using Stock_Manager.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Stock_Manager.ViewModel
{
    /// <summary>
    /// A line in the Account, i.e. a Stock.
    /// </summary>
    public class AccountLineViewModel
    {
        public int RowId { get; set; }
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        public Accounts Account { get; set; }
        [Display(Name = "Stock")]
        public string StockTicker { get; set; }
        public Stocks Stock { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// Total value of holding.
        /// </summary>
        [Display(Name = "Market Value")]
        public double Value { get; set; }
        /// <summary>
        /// Average purchase price for asset.
        /// </summary>
        public double PurchasePrice { get; set; }
        public double Revenue { get; set; } = 1337;
    }
}
