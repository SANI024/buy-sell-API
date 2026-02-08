using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;

namespace buy_sell_project.Services
{
    public class PurchaseService
    {
        private readonly AppDbContext _context;
        public PurchaseService(AppDbContext context) => _context = context;

        public async Task<Purchase> CreatePurchaseAsync(PurchaseDto dto)
        {
            var purchase = new Purchase
            {
                SupplierId = dto.SupplierId,
                Date = DateTime.Now,
                Items = dto.Items.Select(i => new PurchaseItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            foreach (var item in purchase.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                    product.Quantity += item.Quantity;
            }

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }
    }
}
