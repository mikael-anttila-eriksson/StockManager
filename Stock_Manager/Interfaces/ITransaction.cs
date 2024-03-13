namespace Stock_Manager.Interfaces
{
    public interface ITransaction
    {
        int RowId { get; set; }
        int Quantity { get; set; }
        double Price { get; set; }
        DateTime Date { get; set; }
    }
}
