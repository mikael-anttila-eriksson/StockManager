namespace Stock_Manager
{
    /// <summary>
    /// My fast way to hide connectionString when putting this on GitHub...
    /// </summary>
    public static class DatabaseConnection
    {
        private readonly static string _connectionString = "Your ConnectionString";
        public static string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
