using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_Manager.ModelMethods;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Diagnostics;
using System.Globalization;

namespace Stock_Manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult TodayOverview()
        {            
            // // // AddStockData();
            return View();
        }
        /// <summary>
        /// Add stock data from csv-files.
        /// </summary>
        private void AddStockData() ///////////// OBS!! Only Run when adding NEW DATA!! ///////////////////////
        {
            // I got stock  quotes from Yahoo Finance, downloaded csv-files
            string stockToReadData = "";//"ERIC B"; // Stock Ticker "symbol", is the Stock ID in database, must match!
            string fileName = "";//"ERICB";  // NAME of csv-file in Folder ...\\Projekt\\Data\\{NAME}.csv
            List<string[]> input = StockChartMethods.ReadFileCSV(fileName, out string errorMsg1);
            int count = StockChartMethods.InsertStockPrices(input, stockToReadData, out string errorMsg2);
        }
        [HttpGet]
        public IActionResult RegisterUser()
        {
            RegisterViewModel response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public IActionResult RegisterUser(RegisterViewModel response)
        {
            // Send back if Requirements no correct
            if(!ModelState.IsValid) return View(response);

            // No Error - Go to next step   
            string jsString = JsonConvert.SerializeObject(response);
            HttpContext.Session.SetString("user", jsString);
            // Send to HttpGet-Action
            return RedirectToAction("RegistrateUserAndAccount");

        }
        
        [HttpGet]
        public IActionResult RegistrateUserAndAccount()
        {            
            // Save form input
            AccountsViewModel account = new AccountsViewModel();
            return View(account);            
        }
        [HttpPost]
        public IActionResult RegistrateUserAndAccount(AccountsViewModel newAccount)       
        {
            //Receive and save data
            int rowsAffected = 0;

            
            // Open regis
            string jsString = HttpContext.Session.GetString("user");
            RegisterViewModel newUser = new();
            newUser = JsonConvert.DeserializeObject<RegisterViewModel>(jsString);

            // Check if user email exist
            if (UserMethods.GetUser(newUser.Email, out string errorMsg1) != null)
            {
                // User email exist!
                ViewBag.userExist = "This email address is already in use!";
                ViewBag.error = errorMsg1;
                return View("RegisterUser", newUser);
            }
            // Catch any problem
            ViewBag.error = errorMsg1;

            // Add Account-info to RegisterViewModel
            newUser.AccountName = newAccount.Name;
            newUser.Saldo = newAccount.Saldo;

            //Save to database
            rowsAffected = UserMethods.RegisterUser(newUser, out string errorMsg2);
            if(rowsAffected == 2)
            {
                // Correct
                
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Error
                ViewBag.error += $"\nError at registration{errorMsg2}";
            }
            // Create a new User
            //              Account
            return View();
        }
        public IActionResult Privacy()
        {
            return View("TodayOverview");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}