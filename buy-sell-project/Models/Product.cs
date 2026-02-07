namespace buy_sell_project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }
    }
}
