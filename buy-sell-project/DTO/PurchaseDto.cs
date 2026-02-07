namespace buy_sell_project.DTO
{
    public class PurchaseDto
    {
        public int SupplierId { get; set; }
        public List<PurchaseItemDto> Items { get; set; }
    }
}
