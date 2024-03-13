using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Stock_Manager.ModelMethods
{
    public static class StockChartMethods
    {
        private static string _connectionString = DatabaseConnection.ConnectionString;
        public static List<StockChart> GetChartByStock(string ticker, out string errorMsg)
        {
            // Create SQL-Connection
            SqlConnection dbConnection = new();
            // Connect to SQL-Server
            dbConnection.ConnectionString = _connectionString;
            // SQL-Query
            string sqlString = "Select * from Tbl_StockChart where Ch_Stock =@stock";
            // Create T-SQL to execute againts SQL-Server
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
            dbCommand.Parameters.Add("stock", SqlDbType.VarChar, 8).Value = ticker;

            // Read data
            SqlDataReader reader = null;
            errorMsg = "";
            List<StockChart> chart = new();

            try
            {
                // Open
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                // Start read
                while (reader.Read())
                {
                    StockChart dataPoint = new();
                    dataPoint.StockTicker = reader["Ch_Stock"].ToString();
                    dataPoint.Date = Convert.ToDateTime(reader["Ch_Date"]);
                    dataPoint.Open = Convert.ToDouble(reader["Ch_Open"]);
                    dataPoint.Close = Convert.ToDouble(reader["Ch_Close"]);
                    dataPoint.High = Convert.ToDouble(reader["Ch_High"]);
                    dataPoint.Low = Convert.ToDouble(reader["Ch_Low"]);
                    // Two ways to solve null-reading
                    dataPoint.Volume = Convert.ToInt32(reader["Ch_Volume"] as int? ?? 0);   //=Zero if null
                    if (!reader.IsDBNull(7))
                    {
                        dataPoint.Volume = Convert.ToInt32(reader["Ch_Volume"]);            // only read if not null
                    }
                    

                    chart.Add(dataPoint);
                }
                // Sort on date
                chart = Sort(chart);
                reader.Close();
                return chart;
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
        /// 
        /// </summary>
        /// <param name="data">List of string-arrays (rows), format [Date,Open,High,Low,Close,Adj Close,Volume]. AdjClose is NOT USED!</param>
        /// <param name="ticker"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static int InsertStockPrices(List<string[]> data, string ticker, out string errorMsg)
        {
            // Create SQL-Connection
            SqlConnection dbConnection = new();
            // Connect to SQL-Server
            dbConnection.ConnectionString = _connectionString;
            // SQL-Query
            //string sqlString = "Select * from Tbl_StockChart where Ch_Stock =@stock";
            // Create T-SQL to execute againts SQL-Server
            SqlCommand dbCommand = new SqlCommand();
            // Configurate Command
            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = "spInsertDayPrice";
            dbCommand.CommandType = CommandType.StoredProcedure;
            // -----
            
            // Do Transaction
            SqlTransaction transaction;
            int rowsAffected = 0;
            errorMsg = "";
            List<SqlParameter> sqlParameters = new();
            // Add all parameters
            sqlParameters = AddParameters(dbCommand);

            // Open
            dbConnection.Open();
            transaction = dbConnection.BeginTransaction();
            dbCommand.Transaction = transaction;
            int count = 0;
            try
            {
                foreach (string[] row in data)
                {
                    //dbCommand = AddDataCommand(dbCommand, row);
                    sqlParameters = SetParameterValues(sqlParameters, row, ticker);
                    count = dbCommand.ExecuteNonQuery();
                    rowsAffected += count;
                }
                transaction.Commit();
                
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                transaction.Rollback();                
            }
            finally
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
            return rowsAffected;
        }
        //---------------------------------------------------------------
        /// <summary>
        /// Set a list of parameters to the input SQL-Command.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static List<SqlParameter> AddParameters(SqlCommand cmd) 
        {
            List<SqlParameter> sqlParameter = new();
            
            // Set data
            sqlParameter.Add(cmd.Parameters.Add("@stock", SqlDbType.VarChar, 8));
            sqlParameter.Add(cmd.Parameters.Add("@date", SqlDbType.Date));
            sqlParameter.Add(cmd.Parameters.Add("@open", SqlDbType.Decimal, 10));
            sqlParameter.Add(cmd.Parameters.Add("@close", SqlDbType.Decimal, 10));
            sqlParameter.Add(cmd.Parameters.Add("@high", SqlDbType.Decimal, 10));
            sqlParameter.Add(cmd.Parameters.Add("@low", SqlDbType.Decimal, 10));
            sqlParameter.Add(cmd.Parameters.Add("@volume", SqlDbType.Int));

            return sqlParameter;
        }
        /// <summary>
        /// Set new values to the parameters
        /// </summary>
        /// <param name="list"></param>
        /// <param name="row">Row has format [Date,Open,High,Low,Close,Adj Close,Volume]</param>
        /// <param name="stock">The stock to downlad data to</param>
        /// <returns></returns>
        private static List<SqlParameter> SetParameterValues(List<SqlParameter> list, string[] row, string stock)
        {
            // Configure conversion
            NumberFormatInfo provider = new();
            provider.CurrencyDecimalSeparator = "."; // To be able to convert the strings to double.
            // Set values                                       // [Paramter name]
            list[0].Value = stock;                              // [Stock]
            list[1].Value = Convert.ToDateTime(row[0]);         // [Date]
            list[2].Value = Convert.ToDouble(row[1], provider); // [Open]
            list[3].Value = Convert.ToDouble(row[4], provider); // [Close]
            list[4].Value = Convert.ToDouble(row[2], provider); // [High]
            list[5].Value = Convert.ToDouble(row[3], provider); // [Low]
            list[6].Value = Convert.ToInt32(row[6]);            // [Volume]
            //row[5] "ADJ Close" is NOT USED!

            return list;
        }
       
        #region Helpers
        public static List<CandleStickViewModel> TransformToViewModel(List<StockChart> dataset)
        {
            List<CandleStickViewModel> charts = new List<CandleStickViewModel>();
            
            foreach(StockChart day in dataset)
            {
                CandleStickViewModel dataPoint = new();
                double[] array = new double[4];
                array[0] = day.Open;
                array[1] = day.High;                
                array[2] = day.Low;
                array[3] = day.Close;

                dataPoint.Day = day.Date.ToString("yyyy-MM-dd");
                //dataPoint.Day = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day);
                dataPoint.dayPrice = array;

                dataPoint.Open = day.Open;
                dataPoint.High = day.High;
                dataPoint.Low = day.Low;
                dataPoint.Close = day.Close;
                dataPoint.Volume = day.Volume;

                charts.Add(dataPoint);
            }
            return charts;
        }
        /// <summary>
        /// Sort price action by day, ascending.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<StockChart> Sort(List<StockChart> list)
        {
            IEnumerable<StockChart> charts = list.OrderBy(x => x.Date);
            list = charts.ToList();
            //list = (List<StockChart>)(IEnumerable<StockChart>)list.OrderBy(s => s.Date);
            return list;
        }
        #endregion Helpers
        //---------------------------------------------------------------
        public static List<string[]> ReadFileCSV(string fileName, out string errorMsg)
        {
            // Test read
            errorMsg = "";
            string filePath = $"C:\\YOUR_PATH\\{fileName}.csv";
            List<string[]> list = new List<string[]>();
            StreamReader reader = null;
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    reader = new StreamReader(System.IO.File.OpenRead(filePath));
                    //reader = new StreamReader(filePath);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] row = line.Split(",");
                        list.Add(row);
                       
                    }
                    list.RemoveAt(0);
                    list = RemoveNullData(list, out int count);
                    return list;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return null;
                }
                finally
                {
                    reader.Close();
                }


            }
            else
            {
                // no file
                return null;
            }
        }
        /// <summary>
        /// Remove rows of string[], where any data is text = "null".
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<string[]> RemoveNullData(List<string[]> input, out int countNulls)
        {
            countNulls = 0;
            for(int i = 0; i < input.Count; i++)
            {
                string[] row = input[i];
                for(int j = 0; j < row.Length; j++)
                {
                    string cell = row[j];
                    if(cell == "null")
                    {
                        input.RemoveAt(i);
                        countNulls++;
                        break;
                    }
                }
            }
            return input;
            
        }
        
    }
}
