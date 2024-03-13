namespace Stock_Manager.Models
{
    public class Stocks
    {
        public string TickerSymbol { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Todays price, aka closing price.
        /// </summary>
        public double Price { get; set; }
    }
}
