using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sell_project.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) => _context = context;

        public async Task<Product> AddProductAsync(ProductSupplierDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                PurchasePrice = dto.PurchasePrice,
                Quantity = dto.Quantity
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<ProductDto>> GetProductsForCustomerAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    SalePrice = p.SalePrice,
                    Quantity = p.Quantity
                })
                .ToListAsync();

        }

        public async Task<List<ProductSupplierDto>> GetProductsForAdminAsync()
        {
            return await _context.Products
                .Select(p => new ProductSupplierDto
                {
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    Quantity = p.Quantity
                }).ToListAsync();
        }
    }
}

