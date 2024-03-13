using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Windows.Input;

namespace Stock_Manager.ModelMethods
{
    public static class AccountMethods
    {
        private static string _connectionString = DatabaseConnection.ConnectionString;
        /// <summary>
        /// Get all accounts.
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static List<Accounts> GetAllAccounts(out string errorMsg)
        {
            SqlConnection dbConnection = new();
            dbConnection.ConnectionString = _connectionString;
            string sqlString = "Select * from Tbl_Account";

            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

            // Read data
            List<Accounts> accounts = new();
            accounts = ReadFromDatabase(dbCommand, out string errorMsg2);
            errorMsg = errorMsg2;
            return accounts;           
            
        }
        /// <summary>
        /// Get account at ID.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static Accounts GetAccountById(int accountId, out string errorMsg)
        {
            SqlConnection dbConnection = new();
            dbConnection.ConnectionString = _connectionString;
            string sqlString = "Select * from Tbl_Account where Ac_Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = accountId;

            
            

            // Read data
            SqlDataReader reader = null;
            errorMsg = "";

            try
            {
                // Open
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                reader.Read(); // Läs data
                // Start read
                Accounts accRow = new Accounts();
                accRow.AccountId = Convert.ToInt32(reader["Ac_Id"]);
                accRow.UserId = Convert.ToInt32(reader["Ac_User"]);
                accRow.Name = reader["Ac_Name"].ToString();
                accRow.NumOfStocks = Convert.ToInt32(reader["Ac_NumStocks"]);
                accRow.TotalValueAccount = Convert.ToDouble(reader["Ac_Value_Account"]);
                accRow.Saldo = Convert.ToDouble(reader["Ac_Saldo"]);

                reader.Close();
                return accRow;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        /// <summary>
        /// Get a ViewModel list of users accounts.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="errorMsg"></param>
        /// <returns>
        /// OBS. AccountViewModel
        /// </returns>
        public static List<AccountsViewModel> GetUserAccounts(int userId, out string errorMsg)
        {
            SqlConnection dbConnection = new();
            dbConnection.ConnectionString = _connectionString;
            string sqlString = "Select * from Tbl_Account where Ac_User=@id";

            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = userId;

            // Read data
            List<Accounts> accounts = new();
            accounts = ReadFromDatabase(dbCommand, out string errorMsg2);
            errorMsg = errorMsg2;

            // Change to ViewModel
            List<AccountsViewModel> vmAccounts = new();
            vmAccounts = TransformToVM(accounts);
            return vmAccounts;

        }
        
        public static List<AccountsViewModel> TransformToVM(List<Accounts> account)
        {
            List<AccountsViewModel> vmAccount = new List<AccountsViewModel>();
            foreach (Accounts acc in account)
            {
                AccountsViewModel vmAcc = new()
                {
                    AccountId = acc.AccountId,
                    UserId = acc.UserId,
                    Name = acc.Name,
                    NumOfStocks = acc.NumOfStocks,
                    ValueAccount = acc.TotalValueAccount,
                    Saldo = acc.Saldo,
                    Balance = 696,
                };
                vmAccount.Add(vmAcc);
            }
            return vmAccount;
        }
        private static double SetValueAccount(int accountId, out string errorMsg)
        {
            // get account
            Accounts account = new();
            account = GetAccountById(accountId, out string errorMsg1);
            double value = 0;
            List<AccountLine> lines = AccountLineMethods.GetLinesByAccount(account.AccountId, out string errorMsg2);
            foreach (AccountLine line in lines)
            {
                value += line.ValueAsset;
            }
            //save errors
            errorMsg = $"Error 1: {errorMsg1}\nError 2: {errorMsg2}";
            // Calculate balance
            return value;
        }
        //---------------------------------------------------------------
        #region CRUD
        /// <summary>
        /// Add an account to Signed in User.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static int AddAccount(AccountsViewModel account, out string errorMsg)
        {
            // Create SQL-Connection
            SqlConnection dbConnection = new();
            // Connect to SQL-Server
            dbConnection.ConnectionString = _connectionString;
            // SQL-Query
            string sqlString = "Insert into Tbl_Account " +
                "(Ac_User, Ac_Name, Ac_NumStocks, Ac_Value_account, Ac_Saldo)" +
                "values(@userId, @name, @num, @value, @saldo)";
            // Create T-SQL to execute againts SQL-Server
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            dbCommand.Parameters.Add("userId", SqlDbType.Int).Value = account.UserId;
            dbCommand.Parameters.Add("name", SqlDbType.VarChar, 30).Value = account.Name;
            dbCommand.Parameters.Add("num", SqlDbType.Int).Value = account.NumOfStocks;
            dbCommand.Parameters.Add("value", SqlDbType.Decimal, 12).Value = account.ValueAccount;
            dbCommand.Parameters.Add("saldo", SqlDbType.Decimal, 12).Value = account.Saldo;

            // Insert into Database
            int rowsAffected = 0;
            rowsAffected = ExecuteCommandToDatabase(dbCommand, out errorMsg);

            return rowsAffected;
        }
        public static int DeleteAccount(int id, out string errorMsg)
        {
            // Create SQL-Connection
            SqlConnection dbConnection = new();
            // Connect to SQL-Server
            dbConnection.ConnectionString = _connectionString;
            // SQL-Query
            string sqlString = "Delete from Tbl_Account where Ac_Id = @id;";
            // Create T-SQL to execute againts SQL-Server
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            dbCommand.Parameters.Add("Id", SqlDbType.Int).Value = id;

            //Delete
            int rowsAffected = 0;
            rowsAffected = ExecuteCommandToDatabase(dbCommand, out errorMsg);

            return rowsAffected;

        }
        public static int UpdateAccount(AccountsViewModel account, out string errorMsg)
        {
            // Create SQL-Connection
            SqlConnection dbConnection = new();
            // Connect to SQL-Server
            dbConnection.ConnectionString = _connectionString;
            // SQL-Query
            string sqlString = "Update Tbl_Account " +
                "set Ac_Name=@name, Ac_Value_account=@value, Ac_Saldo=@saldo where Ac_Id=@id";
            // Create T-SQL to execute againts SQL-Server
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = account.AccountId;
            dbCommand.Parameters.Add("name", SqlDbType.VarChar, 30).Value = account.Name;
            dbCommand.Parameters.Add("value", SqlDbType.Decimal, 12).Value = SetValueAccount(account.AccountId, out string errorMsg1);
            dbCommand.Parameters.Add("saldo", SqlDbType.Decimal, 12).Value = account.Saldo;

            // Insert into Database
            int rowsAffected = 0;
            rowsAffected = ExecuteCommandToDatabase(dbCommand, out string errorMsg2);
            //save errors
            errorMsg = $"Error 1: {errorMsg1}\nError 2: {errorMsg2}";
            return rowsAffected;
        }
        #endregion CRUD
        //---------------------------------------------------------------
        #region Database Calls
        /// <summary>
        /// Execute Insert/Delete/Update of SQL-Command to database.
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        private static int ExecuteCommandToDatabase(SqlCommand dbCommand, out string errorMsg)
        {
            // Insert into Database
            try
            {
                // open
                dbCommand.Connection.Open();
                int rowsAffected = 0;
                rowsAffected = dbCommand.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    // No Error
                    errorMsg = "";
                }
                else
                {
                    // Error
                    errorMsg = "Error trying to insert User to database";
                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return 0; // no row affected
            }
            finally
            {
                dbCommand.Connection.Close();
            }
        }
        /// <summary>
        /// Reads a list of Accounts from database.
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        private static List<Accounts> ReadFromDatabase(SqlCommand dbCommand, out string errorMsg)
        {
            // Read data
            SqlDataReader reader = null;
            errorMsg = "";
            List<Accounts> accounts = new List<Accounts>();

            try
            {
                // Open
                dbCommand.Connection.Open();
                reader = dbCommand.ExecuteReader();

                // Start read
                while (reader.Read())
                {
                    Accounts accRow = new Accounts();
                    accRow.AccountId = Convert.ToInt32(reader["Ac_Id"]);
                    accRow.UserId = Convert.ToInt32(reader["Ac_User"]);
                    accRow.Name = reader["Ac_Name"].ToString();
                    accRow.NumOfStocks = Convert.ToInt32(reader["Ac_NumStocks"]);
                    accRow.TotalValueAccount = Convert.ToDouble(reader["Ac_Value_Account"]);
                    accRow.Saldo = Convert.ToDouble(reader["Ac_Saldo"]);

                    accounts.Add(accRow);
                }
                reader.Close();
                return accounts;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                dbCommand.Connection.Close();
            }
        }
        #endregion Database Calls
        //---------------------------------------------------------------
    }
}
