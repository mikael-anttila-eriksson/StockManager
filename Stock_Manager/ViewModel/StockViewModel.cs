using Stock_Manager.Models;
using System.ComponentModel.DataAnnotations;

namespace Stock_Manager.ViewModel
{
    public class StockViewModel
    {
        //------------- From Stocks ----------------------------------------------
        [Display(Name = "Stocks")]
        public string TickerSymbol { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Todays price, aka closing price.
        /// </summary>
        [Display(Name = "Recent price")]
        public double Price { get; set; }

        //------------- From StockChart ----------------------------------------------
        public List<StockChart>? StockChart { get; set; }
    }
}
