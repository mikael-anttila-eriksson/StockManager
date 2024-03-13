using Stock_Manager.Interfaces;

namespace Stock_Manager.Models
{
    public class StockSold : ITransaction
    {
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
        public DateTime Date { get; set; }
        public double Curtage { get; set; }
    }
}
