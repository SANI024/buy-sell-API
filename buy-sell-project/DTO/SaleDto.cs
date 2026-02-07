namespace buy_sell_project.DTO
{
    public class SaleDto
    {
        public int CustomerId { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }
}
