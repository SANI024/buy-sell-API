using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sell_project.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDbContext _context;

        public SaleService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<(bool Success, string Message)> CreateSale(int productId, int quantity, int userId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null)
                return (false, "Customer not found");

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                return (false, "Product not found");

            if (!product.IsActive)
                return (false, "Product is not available");

            if (product.Quantity < quantity)
                return (false, $"Not enough stock. Available quantity: {product.Quantity}");

            
            product.Quantity -= quantity;

            var sale = new Sale
            {
                CustomerId = customer.Id,
                Date = DateTime.Now,
                Items = new List<SaleItem>
        {
            new SaleItem
            {
                ProductId = productId,
                Quantity = quantity,
                SalePrice = product.SalePrice
            }
        }
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return (true, "Purchase successful");
        }

        public async Task<List<Product>> GetAvailableProducts()
        {
           return await _context.Products
                .Where(p=>p.IsActive && p.Quantity>0)
                .ToListAsync();
        }
       

    }
}
