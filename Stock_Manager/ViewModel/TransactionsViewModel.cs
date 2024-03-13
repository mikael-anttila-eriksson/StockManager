using Stock_Manager.Models;
using System.ComponentModel.DataAnnotations;

namespace Stock_Manager.ViewModel
{
    public class TransactionsViewModel
    {
        /// <summary>
        /// True for Buy-Transaction. False for Sell-Transaction
        /// </summary>
        public bool IsBuy { get; set; }
        public int RowId { get; set; }
        public int AccountId { get; set; }
        public Accounts Account { get; set; }
        public string StockTicker { get; set; }
        public Stocks Stock { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        /// <summary>
        /// Day of trade.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy'-'MM'-'dd}")]
        public DateTime Date { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public double Curtage { get; set; }
    }
}
