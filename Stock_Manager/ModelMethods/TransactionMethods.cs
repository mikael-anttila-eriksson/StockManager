using Stock_Manager.Interfaces;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Stock_Manager.ModelMethods;

/// <summary>
/// Methods for stock transactions.
/// </summary>
public class TransactionMethods
{
    private static string _connectionString = DatabaseConnection.ConnectionString;
    //---------------------------------------------------------------
    #region Buy
    public static List<StockPurchases> GetAllBuys(out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_Purchases";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";
        List<StockPurchases> boughts = new List<StockPurchases>();

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            while (reader.Read())
            {
                StockPurchases buy = new();
                buy.RowId = Convert.ToInt32(reader["Pu_RowId"]);
                buy.AccountId = Convert.ToInt32(reader["Pu_Account"]);
                buy.StockTicker = reader["Pu_Stock"].ToString();
                buy.Quantity = Convert.ToInt32(reader["Pu_Quantity"]);
                buy.Price = Convert.ToDouble(reader["Pu_Price"]);
                buy.Date = Convert.ToDateTime(reader["Pu_Date"]); //.ToString()
                buy.Curtage = Convert.ToDouble(reader["Pu_Courtage"]);

                boughts.Add(buy);
            }
            reader.Close();
            return boughts;
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
    /// Get all buy-transactions for account.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="errorMsg"></param>
    /// <returns>
    /// null if error
    /// </returns>
    public static List<StockPurchases> GetBuysByAccount(int accountId,out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_Purchases where Pu_Account=@id";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("id", System.Data.SqlDbType.Int).Value = accountId;

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";
        List<StockPurchases> boughts = new List<StockPurchases>();

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            while (reader.Read())
            {
                StockPurchases buy = new();
                buy.RowId = Convert.ToInt32(reader["Pu_RowId"]);
                buy.AccountId = Convert.ToInt32(reader["Pu_Account"]);
                buy.StockTicker = reader["Pu_Stock"].ToString();
                buy.Quantity = Convert.ToInt32(reader["Pu_Quantity"]);
                buy.Price = Convert.ToDouble(reader["Pu_Price"]);
                buy.Date = Convert.ToDateTime(reader["Pu_Date"]); //.ToString()
                buy.Curtage = Convert.ToDouble(reader["Pu_Courtage"]);

                boughts.Add(buy);
            }
            reader.Close();
            return boughts;
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
    //public static List<Stocktr> TransformToVM(List<Stocks> stocks)
    //{
    //    List<StockViewModel> vmStocks = new List<StockViewModel>();
    //    foreach (Stocks asset in stocks)
    //    {
    //        StockViewModel vmAsset = new()
    //        {
    //            TickerSymbol = asset.TickerSymbol,
    //            Name = asset.Name,
    //            Price = asset.Price,
    //        };
    //        vmStocks.Add(vmAsset);
    //    }
    //    return vmStocks;
    //}
    #endregion Buy
    //---------------------------------------------------------------
    #region Sell
    public static List<StockSold> GetAllSells(out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_Sold";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";
        List<StockSold> sold = new List<StockSold>();

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            while (reader.Read())
            {
                StockSold sell = new();
                sell.RowId = Convert.ToInt32(reader["Pu_RowId"]);
                sell.AccountId = Convert.ToInt32(reader["Pu_Account"]);
                sell.StockTicker = reader["Pu_Stock"].ToString();
                sell.Quantity = Convert.ToInt32(reader["Pu_Quantity"]);
                sell.Price = Convert.ToDouble(reader["Pu_Price"]);
                sell.Date = Convert.ToDateTime(reader["Pu_Date"]); //.ToString()
                sell.Curtage = Convert.ToDouble(reader["Pu_Courtage"]);

                sold.Add(sell);
            }
            reader.Close();
            return sold;
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
    /// Get all sell-transactions for account.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="errorMsg"></param>
    /// <returns>
    /// null if error.
    /// </returns>
    public static List<StockSold> GetSellsByAccount(int accountId, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_Sold where So_Account=@id";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("id", System.Data.SqlDbType.Int).Value = accountId;

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";
        List<StockSold> sold = new List<StockSold>();

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            while (reader.Read())
            {
                StockSold sell = new();
                sell.RowId = Convert.ToInt32(reader["So_RowId"]);
                sell.AccountId = Convert.ToInt32(reader["So_Account"]);
                sell.StockTicker = reader["So_Stock"].ToString();
                sell.Quantity = Convert.ToInt32(reader["So_Quantity"]);
                sell.Price = Convert.ToDouble(reader["So_Price"]);
                sell.Date = Convert.ToDateTime(reader["So_Date"]); //.ToString()
                sell.Curtage = Convert.ToDouble(reader["So_Courtage"]);

                sold.Add(sell);
            }
            reader.Close();
            return sold;
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

    #endregion Sell    
    //---------------------------------------------------------------
    #region Make Transaction List
    /// <summary>
    /// Get all trqansaction for specified account.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public static IEnumerable<TransactionsViewModel> TransactionList(int accountId, out string errorMsg)
    {
        IEnumerable<TransactionsViewModel> transactions = new List<TransactionsViewModel>();
        List<StockPurchases> bought = new();
        // Ändra till getbyId ...
        // Get all transactions (Buy/Sell) from account x:
        bought = GetBuysByAccount(accountId, out string errorMsg1);
        List<StockSold> sold = new List<StockSold>();
        sold = GetSellsByAccount(accountId, out string errorMsg2);

        // Merge these 2 lists into one
        if(bought != null && sold != null)
        {
            transactions = MergeBuySellToOneList(bought, sold);
        }
        
        // Order Transactions by date
        transactions = transactions.OrderBy(m => m.Date);

        // Save error messages, after each other.
        errorMsg = errorMsg1 + "\n" + errorMsg2;

        return transactions;
    }
    /// <summary>
    /// Merge Buy- and Sell-transactions into one transaction-list.
    /// </summary>
    /// <param name="bought"></param>
    /// <param name="sold"></param>
    /// <returns></returns>
    private static List<TransactionsViewModel> MergeBuySellToOneList(List<StockPurchases> bought, List<StockSold> sold)
    {
        List<TransactionsViewModel> transactions = new List<TransactionsViewModel>();
        // cannot merge ID:s , duplicates
        // -> Make a new id
        int id = 1;
        // Add Bought stocks
        foreach (StockPurchases buy in bought)
        {
            TransactionsViewModel trans = new TransactionsViewModel()
            {
                IsBuy = true,       // Mark it as a BUY
                RowId = id++,
                AccountId = buy.AccountId,
                StockTicker = buy.StockTicker,
                Quantity = buy.Quantity,
                Price = buy.Price,
                Date = buy.Date,
                Curtage = buy.Curtage
            };
            transactions.Add(trans);
        }
        // Add sold stocks
        foreach (StockSold sell in sold)
        {
            TransactionsViewModel trans = new TransactionsViewModel()
            {
                IsBuy = false,       // Mark it as a SELL
                RowId = id++,
                AccountId = sell.AccountId,
                StockTicker = sell.StockTicker,
                Quantity = sell.Quantity,
                Price = sell.Price,
                Date = sell.Date,
                Curtage = sell.Curtage
            };
            transactions.Add(trans);
        }

        return transactions;
    }

    #endregion Make Transaction List
    //---------------------------------------------------------------


}
