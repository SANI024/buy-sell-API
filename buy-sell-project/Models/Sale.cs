namespace buy_sell_project.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<SaleItem> Items { get; set; }
    }
}
