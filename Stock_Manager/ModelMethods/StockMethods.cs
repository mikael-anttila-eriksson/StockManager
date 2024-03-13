using Microsoft.AspNetCore.Mvc.Infrastructure;
using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Data;
using System.Data.SqlClient;

namespace Stock_Manager.ModelMethods;

public class StockMethods
{
    private static string _connectionString = DatabaseConnection.ConnectionString;
    public static List<Stocks> GetAllStocks(out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_Stocks";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";
        List<Stocks> stocks = new List<Stocks>();

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            while (reader.Read())
            {
                Stocks aStock = new();
                aStock.TickerSymbol = reader["St_Ticker_Symbol"].ToString();
                aStock.Name = reader["St_Name"].ToString();
                aStock.Price = Convert.ToDouble(reader["St_Price"]);

                stocks.Add(aStock);
            }
            reader.Close();
            return stocks;
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
    public static Stocks GetStock(string ticker, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Select * from Tbl_Stocks where St_Ticker_Symbol =@ticker";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("ticker", SqlDbType.VarChar, 8).Value = ticker;

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            // Start read
            reader.Read();// This one is IMPORTANT!!!
            Stocks aStock = new();
            aStock.TickerSymbol = reader["St_Ticker_Symbol"].ToString();
            aStock.Name = reader["St_Name"].ToString();
            aStock.Price = Convert.ToDouble(reader["St_Price"]);

            reader.Close();
            return aStock;
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
    
    //---------------------------------------------------------------
    public static List<StockViewModel> TransformListToVM(List<Stocks> stocks)
    {
        List<StockViewModel> vmStocks = new List<StockViewModel>();
        foreach (Stocks asset in stocks)
        {
            StockViewModel vmAsset = new()
            {
                TickerSymbol = asset.TickerSymbol,
                Name = asset.Name,
                Price = asset.Price,
            };
            vmStocks.Add(vmAsset);
        }
        return vmStocks;
    }
    public static StockViewModel TransformToVM(Stocks stock)
    {
        StockViewModel vmStock = new()
        {
            TickerSymbol = stock.TickerSymbol,
            Name = stock.Name,
            Price = stock.Price
        };        
        return vmStock;
    }
    //---------------------------------------------------------------
    #region Prices
    /// <summary>
    /// Get latest price for specified stock.
    /// </summary>
    /// <param name="stock"></param>
    /// <returns>
    /// Last closing price.
    /// </returns>
    public static double GetLatestPrice(string stock, out string errorMsg)
    {
        // get price-data for stock
        List<StockChart> stockData = StockChartMethods.GetChartByStock(stock, out errorMsg);
        // get latest price from stock
        double latestPrice = stockData.LastOrDefault().Close; // 0.0 if default
        
        return latestPrice;
    }
    /// <summary>
    /// Update current price for all stocks. OBS!! Can generate a long error messages!
    /// </summary>
    /// <param name="errorMsg"></param>
    public static void UpdatePricesAllStocks(out string errorMsg)
    {
        List<Stocks> stocks = new List<Stocks>();
        stocks = GetAllStocks(out string errorMsg1);        
        double price = 0;
        // Index error messages
        int i = 2;

        errorMsg = $"Error1: {errorMsg1}\n";
        foreach (Stocks stock in stocks)
        {
            // Set new price
            price = GetLatestPrice(stock.TickerSymbol, out string errorMsg2);
            UpdatePriceStock(stock.TickerSymbol, price, out string errorMsg3);
            errorMsg += $"Error{i++}: {errorMsg2}\nError{i++}: {errorMsg3}";
            //i++;
        }

    }
    public static int UpdatePriceStock(string ticker, double price, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Update Tbl_Stocks set St_price=@price where St_Ticker_Symbol =@ticker";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("ticker", SqlDbType.VarChar, 8).Value = ticker;
        dbCommand.Parameters.Add("price", SqlDbType.Decimal, 12-4).Value = price;

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";

        // Update Database
        try
        {
            // open
            dbConnection.Open();
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
                errorMsg = "Error trying to update User to database";
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
            dbConnection.Close();
        }        
    }
    #endregion Prices
    //---------------------------------------------------------------

}
