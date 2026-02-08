namespace buy_sell_project.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public ICollection<PurchaseItem> Items { get; set; }
    }
}
