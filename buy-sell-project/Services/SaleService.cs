using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;

namespace buy_sell_project.Services
{
    public class SaleService
    {
        private readonly AppDbContext _context;
        public SaleService(AppDbContext context) => _context = context;

        public async Task<Sale> CreateSaleAsync(SaleDto dto)
        {
            var sale = new Sale
            {
                CustomerId = dto.CustomerId,
                Date = DateTime.Now,
                Items = dto.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            foreach (var item in sale.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception("Product not found");

                if (product.Quantity < item.Quantity)
                    throw new Exception($"Not enough stock for {product.Name}");

                product.Quantity -= item.Quantity;
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }
    }
}
