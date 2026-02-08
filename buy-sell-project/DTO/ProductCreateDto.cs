namespace buy_sell_project.DTO
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
    }
}
