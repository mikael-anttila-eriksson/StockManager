namespace Stock_Manager
{
    /// <summary>
    /// My fast way to hide connectionString when putting this on GitHub...
    /// </summary>
    public static class DatabaseConnection
    {
        private readonly static string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StockManager;Integrated Security=True";
        public static string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
