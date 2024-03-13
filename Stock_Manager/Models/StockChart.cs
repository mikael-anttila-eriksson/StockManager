namespace Stock_Manager.Models
{
    public record class StockChart
    {
        public int RowId { get; set; }
        public string StockTicker { get; set; } = "None";
        public Stocks Stock { get; set; }
        /// <summary>
        /// The day for the price action.
        /// </summary>
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public int? Volume { get; set; }
    }
}
