using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Data;
using System.Data.SqlClient;

namespace Stock_Manager.ModelMethods;

public static class AccountLineMethods
{
    private static string _connectionString = DatabaseConnection.ConnectionString;
    //---------------------------------------------------------------
    public static List<AccountLine> GetLinesByAccount(int accountId,out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_StockAccount where SA_Acc = @id";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("id", SqlDbType.Int).Value = accountId;

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";
        List<AccountLine> accountLines = new List<AccountLine>();

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            while (reader.Read())
            {
                AccountLine line = new();
                line.RowId = Convert.ToInt32(reader["SA_RowId"]);
                line.AccountId = Convert.ToInt32(reader["SA_Acc"]); 
                line.StockTicker = reader["SA_Stock"].ToString();
                line.Quantity = Convert.ToInt32(reader["SA_Quantity"]);
                line.ValueAsset = Convert.ToDouble(reader["SA_Value"]);
                 
                accountLines.Add(line);
            }
            reader.Close();
            return accountLines;
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
    public static List<AccountLineViewModel> TransformToVM(List<AccountLine> account)
    {
        List<AccountLineViewModel> vmAccount = new List<AccountLineViewModel>();
        foreach(AccountLine line in account)
        {
            AccountLineViewModel vmLine = new()
            {
                RowId = line.RowId,
                AccountId = line.AccountId,
                //Account = line.Account,
                StockTicker = line.StockTicker,
                //Stock = line.Stock,
                Quantity = line.Quantity,
                Value = line.ValueAsset
            };
            vmAccount.Add(vmLine);
        }
        return vmAccount;
    }
    /// <summary>
    /// Get ViewModel AccountLines directly, by account ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="errorMsg"></param>
    /// <returns>
    /// OBS. ViewModel
    /// </returns>
    public static List<AccountLineViewModel> GetVMLineByAccount(int accountId, out string errorMsg)
    {
        List<AccountLine> lines = new();
        lines = GetLinesByAccount(accountId, out errorMsg);
        // transform to ViewModel
        List<AccountLineViewModel> vmLines = new();
        vmLines = TransformToVM(lines);
        return vmLines;
    }
    //---------------------------------------------------------------
    /// <summary>
    /// Get the new the value (Quantity * Price) of the Asset on specified account.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>
    /// The new value of asset.
    /// </returns>
    public static double GetValueAsset(int accountId, Stocks stock)
    {
        double value = 0;
        double price = 0;
        int quantity = 0;
        // get account lines
        List<AccountLine> lines = new List<AccountLine>();
        lines = GetLinesByAccount(accountId, out string errorMsg);
        // Get how many stocks the account have
        quantity = lines.SingleOrDefault(s => s.StockTicker == stock.TickerSymbol).Quantity;
        // get latest price from stock
        price = StockMethods.GetLatestPrice(stock.TickerSymbol, out string errorMsg2);
        value = quantity * price;

        return value;
    }
}
