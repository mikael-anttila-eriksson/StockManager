using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_Manager.ModelMethods;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Linq;
using System.Security.Claims;

namespace Stock_Manager.Controllers
{
    public class AccountController : Controller
    {
        // Fields
        
        /// <summary>
        /// For Cookie Authentication
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        // Constructor
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {            
            _httpContextAccessor = httpContextAccessor;
        }

        // Methods
        // -----------------------------------------------------------------
        /// <summary>
        /// View selected account
        /// </summary>
        /// <returns></returns>
        public IActionResult AccountView(int accountId)         
        {
            
            // Handle error messages
            string error = string.Empty;
            // Get Account
            Accounts account = new();
            account = AccountMethods.GetAccountById(accountId, out string errorMsg1);
            // Transform to ViewModel
            AccountsViewModel vmAccount = new()
            {
                AccountId = account.AccountId,
                UserId = account.UserId,
                Name = account.Name,
                NumOfStocks = account.NumOfStocks,
                ValueAccount = account.TotalValueAccount,
                Saldo = account.Saldo,
                Balance = 696
            };
            // Add account lines, aka the stocks
            vmAccount.VMAccountLines = AccountLineMethods.GetVMLineByAccount(accountId, out string errorMsg2);
            error = $"Error 1: {errorMsg1}\nError 2: {errorMsg2}";
            //vmAccount = AccountMethods.TransformToVM(account)
            //// get lines in Account
            //List<AccountLine> accounts = new();
            //accounts = AccountLineMethods.GetLinesByAccount(accountId ,out string errorMsg3);
            //List<AccountLineViewModel> vmAclinet = new();
            //vmAclinet = AccountLineMethods.TransformToVM(accounts);
            

            // Stats
            ViewBag.error = error;
            ViewBag.account = accountId;
            ViewBag.price = 22;


            return View(vmAccount);
        }
        //---------------------------------------------------------------
        #region CRUD
        [HttpGet]
        public IActionResult AddAccount()
        {
            // Get current user
            string jsString = HttpContext.Session.GetString("currentUser");
            UserAccountsViewModel user = JsonConvert.DeserializeObject<UserAccountsViewModel>(jsString);
            // User found?
            if (user == null)
            {
                // No
                TempData["error"] = "Error: User not found";
                return RedirectToAction("TodayOverview", "Home");
            }
            // Yse --> Fill in new Account info
            AccountsViewModel response = new AccountsViewModel();            
            return View(response);

        }
        [HttpPost]
        public IActionResult AddAccount(AccountsViewModel response)
        {
            // Get current user
            string jsString = HttpContext.Session.GetString("currentUser");
            UserAccountsViewModel user = JsonConvert.DeserializeObject<UserAccountsViewModel>(jsString);
            // set Default values
            response.UserId = user.UserId;
            response.ValueAccount = response.Saldo;
            response.NumOfStocks = 0;
            // Send back if Requirements no correct
            //if (!ModelState.IsValid) return View(response); //funkar inte, blir false

            // Add account
            int rowsAffected = 0;
            rowsAffected = AccountMethods.AddAccount(response, out string errorMsg);
            // Check
            if(rowsAffected == 1)
            {
                // Ok
                return RedirectToAction("Overview", "MyEconomy");
            }
            else
            {
                // error
                TempData["error"] = errorMsg;
                return RedirectToAction("Overview", "MyEconomy");
            }
        }
        public IActionResult DeleteAccount(int accountId)
        {            
            // Delete account
            int rowsAffected = 0;
            rowsAffected = AccountMethods.DeleteAccount(accountId, out string errorMsg);    
            // Check
            if (rowsAffected == 1)
            {
                // Ok
                return RedirectToAction("Overview", "MyEconomy");
            }
            else
            {
                // error
                TempData["error"] = errorMsg;
                return RedirectToAction("Overview", "MyEconomy");
            }

        }
        [HttpGet]
        public IActionResult UpdateAccount(int accountId)
        {
            Accounts account = new();
            account = AccountMethods.GetAccountById(accountId, out string errorMsg);
            AccountsViewModel vmAccount = new AccountsViewModel()
            {
                AccountId = accountId,
                UserId = account.UserId,
                Name = account.Name,
                NumOfStocks = account.NumOfStocks,
                ValueAccount = account.TotalValueAccount,
                Saldo = account.Saldo,
                //Balance = ??
            };

            // Save account
            string jsString = JsonConvert.SerializeObject(vmAccount);
            HttpContext.Session.SetString("updateAccount", jsString);

            return View(vmAccount);
        }
        [HttpPost]
        public IActionResult UpdateAccount(AccountsViewModel response)
        {
            // get account
            string jsString = HttpContext.Session.GetString("updateAccount");
            AccountsViewModel vmAccount = JsonConvert.DeserializeObject<AccountsViewModel>(jsString);
            // Set non-edited values
            response.AccountId = vmAccount.AccountId;
            response.UserId = vmAccount.UserId;

            // Update account
            int rowsAffected = 0;
            rowsAffected = AccountMethods.UpdateAccount(response, out string errorMsg);
            // Check
            if (rowsAffected == 1)
            {
                // Ok
                return RedirectToAction("Overview", "MyEconomy");
            }
            else
            {
                // error
                TempData["error"] = errorMsg;
                return RedirectToAction("Overview", "MyEconomy");
            }

        }
        #endregion CRUD
        //---------------------------------------------------------------
        [HttpGet]
        public IActionResult TransactionsForAccount(int accountId)
        {
            IEnumerable<TransactionsViewModel> transactions = new List<TransactionsViewModel>();
            transactions = TransactionMethods.TransactionList(accountId, out string errorMsg);
            
            // Get account ID
            if(transactions.Count() > 0)
            {
                ViewBag.account = transactions.ElementAt(0).AccountId;
            }
            return View(transactions);
        }
        //---------------------------------------------------------------
        #region Authentication stuff
        [HttpGet]
        public IActionResult Login()
        {
            // Sparar input vid felklick på login
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vmLogin)
        {
            //await SignInUser(vmLogin.Email);
            //return RedirectToAction("TodayOverview", "Home");

            // Vid fel, återsänd sida med input
            if (!ModelState.IsValid) return View(vmLogin);

            // Check if user exist
            AppUser getUser = UserMethods.GetUser(vmLogin.Email, out string errorMsg);
            if(getUser == null)
            {
                // No user found with that email
                ViewBag.loginError = "No user with that email exist.";
                return View(vmLogin);
            }
            else
            {                
                // User found, check password
                if(UserMethods.CheckPassword(getUser, vmLogin.Password, out string loginError))
                {
                    // Password correct
                    //ALT1: CookieAuthentication
                    await SignInUser(getUser.Email);
                    TempData["error"] = loginError; //save possible. error
                    return RedirectToAction("TodayOverview", "Home");
                    //ALT2: Set Inlog-Cookie
                    HttpContext.Response.Cookies.Append("regCookie", "token#inLog");
                    string? cookieID = HttpContext.Request.Cookies["regCookie"];
                }
                else
                {
                    // Not correct
                    ViewBag.loginError = loginError;
                    return View(vmLogin);
                }
            }

        }
        public async Task<IActionResult> SignOut()
        {
            await SignOutUser();
            return RedirectToAction("TodayOverview", "Home");
        }
        //---------------------------------------------------------------
        // Sign IN/OUT
        private async Task SignOutUser()
        {
            await _httpContextAccessor.HttpContext
                .SignOutAsync("MyAuthenticationScheme");
            
        }        
        private async Task SignInUser(string email)      
        {
            var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email),
                };
            var claimsIdentity = new ClaimsIdentity(claims, "MyAuthenticationScheme");
            await _httpContextAccessor.HttpContext
                .SignInAsync("MyAuthenticationScheme",
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties());
        }
        #endregion Authentication stuff
        //---------------------------------------------------------------
    }
}
