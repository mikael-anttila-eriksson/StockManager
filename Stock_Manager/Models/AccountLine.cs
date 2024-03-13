namespace Stock_Manager.Models
{
    /// <summary>
    /// A line in the Account, i.e. a Stock.
    /// </summary>
    public class AccountLine 
    {
        public int RowId { get; set; }
        public int AccountId { get; set; }
        public Accounts Account { get; set; }
        public string StockTicker { get; set; }
        public Stocks Stock { get; set; }
        public int Quantity { get; set; }
        /// <summary>
        /// Total value of holding.
        /// </summary>
        public double ValueAsset { get; set; }
    }
}
