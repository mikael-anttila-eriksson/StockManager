namespace Stock_Manager.Models
{
    public class Accounts
    {
        //Fields

        //---------------------------------------------------------------
        #region Constructor

        #endregion
        //---------------------------------------------------------------
        #region Properties
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        /// <summary>
        /// Optional name for account.
        /// </summary>
        public string? Name { get; set; }
        public int NumOfStocks { get; set; }
        public double TotalValueAccount { get; set; }
        public double Saldo { get; set; }
        #endregion
        //---------------------------------------------------------------
    }
}
