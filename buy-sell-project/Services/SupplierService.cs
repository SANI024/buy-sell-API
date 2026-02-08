using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sell_project.Services
{
    public class SupplierService
    {
        private readonly AppDbContext _context;
        public SupplierService(AppDbContext context) => _context = context;

        public async Task AddProduct(int userId, SupplierProductDto dto)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (supplier == null)
                throw new Exception("Supplier not found");

           
            var existingProduct = await _context.SupplierProduct
                .FirstOrDefaultAsync(p => p.SupplierId == supplier.Id && p.Name == dto.Name && p.PurchasePrice==dto.PurchasePrice);

            if (existingProduct != null)
            {
                
                existingProduct.Quantity += dto.Quantity;
                existingProduct.PurchasePrice = dto.PurchasePrice; 
            }
            else
            {
                
                var product = new SupplierProduct
                {
                    SupplierId = supplier.Id,
                    Name = dto.Name,
                    PurchasePrice = dto.PurchasePrice,
                    Quantity = dto.Quantity
                };
                await _context.SupplierProduct.AddAsync(product);
            }

            await _context.SaveChangesAsync();
        }
    }
}
