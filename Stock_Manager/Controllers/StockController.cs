using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_Manager.ModelMethods;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;

namespace Stock_Manager.Controllers
{
    /// <summary>
    /// Show info about the stock and its Chart of price action.
    /// </summary>
    public class StockController : Controller
    {
        public IActionResult ShowAllStocks()
        {
            List<Stocks> stocks = new List<Stocks>();
            stocks = StockMethods.GetAllStocks(out string errorMsg);
            List<StockViewModel> vmStocks = new();
            vmStocks = StockMethods.TransformListToVM(stocks);

            // Stats
            ViewBag.error = errorMsg;

            return View(vmStocks);
        }
        public IActionResult StockDetail(string ticker)
        {
            Stocks stock = new Stocks();
            stock = StockMethods.GetStock(ticker, out string errorMsg);
            StockViewModel vmStock = new StockViewModel();
            vmStock = StockMethods.TransformToVM(stock);

            // Stats
            ViewBag.error = errorMsg;
            ViewBag.name = JsonConvert.SerializeObject(stock.TickerSymbol);
            return View(vmStock);
        }
        //---------------------------------------------------------------
        public ContentResult MyJSON(string ticker)
        {
            List<StockChart> dataSet = new List<StockChart>();
            List<CandleStickViewModel> vmDataSet = new();
            dataSet = StockChartMethods.GetChartByStock(ticker, out string errorMsg);
            vmDataSet = StockChartMethods.TransformToViewModel(dataSet);

            // Set slider min/max:
            //max = today
            ViewBag.maxDate = dataSet[dataSet.Count - 1].Date.ToString("yyyy-MM-dd");
            //min = today - 3 months
            DateTime min = dataSet[dataSet.Count - 1].Date.AddMonths(-3);
            string minDate = min.ToString("yyyy-MM-dd");
            minDate = dataSet[dataSet.Count - 1].Date.AddMonths(-3).ToString("yyyy-MM-dd");
            ViewBag.minDate = minDate;

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return Content(JsonConvert.SerializeObject(vmDataSet, jsonSettings), "application/json");
        }
        //---------------------------------------------------------------
    }
}
