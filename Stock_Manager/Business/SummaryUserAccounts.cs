using Stock_Manager.ModelMethods;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;

namespace Stock_Manager.Business
{
    public static class SummaryUserAccounts
    {
        public static UserAccountsViewModel GetSummaryUser(int userId, out string errorMsg)
        {
            
            // Users accounts
            UserAccountsViewModel userVMAccounts = new UserAccountsViewModel();
            userVMAccounts.UserAccounts = AccountMethods.GetUserAccounts(userId, out string errorMsg2);
            errorMsg = errorMsg2;
            // Total value
            double totValue = 0;
            // Total saldo
            double totSaldo = 0;
            // Total Balance
            double totBalance = 0;
            foreach (AccountsViewModel account in userVMAccounts.UserAccounts)
            {
                totValue += account.ValueAccount;
                totSaldo += account.Saldo;
                //totBalance += account.Balance;            // Remove "//" when Add Balance feature.
            }
            userVMAccounts.UserId = userId;
            userVMAccounts.TotalValue = totValue;
            userVMAccounts.TotalSaldo = totSaldo;
            //userVMAccounts.TotalBalance = totBalance;
            
            
            return userVMAccounts;    
        }
    }
}
