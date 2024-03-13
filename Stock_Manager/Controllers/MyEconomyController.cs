using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_Manager.Business;
using Stock_Manager.ModelMethods;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;

namespace Stock_Manager.Controllers
{
    // Authorize - Only User inlogged user can access this Controller!
    [Authorize(AuthenticationSchemes = "MyAuthenticationScheme")]
    public class MyEconomyController : Controller
    {
        public IActionResult Overview()
        {
            
            // Check User
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;//is Email used att sign in
                AppUser user = new AppUser();
                user = UserMethods.GetUser(email, out string errorMsg1);

                // User found?
                if(user == null)
                {
                    // user not found
                    TempData["error"] = "Error: User not found";
                    return RedirectToAction("TodayOverview", "Home");
                }
                // User found
                UserAccountsViewModel userVmAccounts = new UserAccountsViewModel();
                userVmAccounts = SummaryUserAccounts.GetSummaryUser(user.UserId, out string errorMsg2);

                // Save User - Used in AccountController
                string jsString = JsonConvert.SerializeObject(userVmAccounts);
                HttpContext.Session.SetString("currentUser", jsString);

                // Stats
                ViewBag.error = errorMsg2;
                return View(userVmAccounts);
            }
            // user is signed out!
            TempData["error"] = "Error: User is signed out!";
            return RedirectToAction("TodayOverview", "Home"); ;

           
        }
        public IActionResult TestOverView()
        {
            return View();
        }
    }
}
