using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sell_project.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)=>_context=context;

        public async Task ActivateProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            product.IsActive = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            product.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<AdminViewSupplierProd>> GetAllSupplierProducts()
        {
            return await _context.SupplierProduct
        .Include(sp => sp.Supplier)
        .Select(sp => new AdminViewSupplierProd
        {
            SupplierProductId = sp.Id,
            SupplierCompanyName = sp.Supplier.CompanyName,
            ProductName = sp.Name,
            PurchasePrice = sp.PurchasePrice,
            Quantity = sp.Quantity
        })
        .ToListAsync();
        }

        public async Task PurchaseFromSupplier(int supplierProductId, int quantity, decimal salePrice)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            var supplierProduct = await _context.SupplierProduct
                .FirstOrDefaultAsync(x => x.Id == supplierProductId);

            if (supplierProduct == null)
                throw new Exception("Supplier product not found");

            if (supplierProduct.Quantity < quantity)
                throw new Exception("Not enough quantity in supplier stock");

            supplierProduct.Quantity -= quantity;

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == supplierProduct.Name);

            if (product == null)
            {
                product = new Product
                {
                    Name = supplierProduct.Name,
                    PurchasePrice = supplierProduct.PurchasePrice,
                    SalePrice = salePrice,
                    Quantity = quantity,
                    IsActive = true
                };

                _context.Products.Add(product);
            }
            else
            {
                product.Quantity += quantity;
                product.IsActive = true;
                product.PurchasePrice = supplierProduct.PurchasePrice;
                product.SalePrice = salePrice;
            }

            var purchase = new Purchase
            {
                Date = DateTime.Now,
                Items = new List<PurchaseItem>
        {
            new PurchaseItem
            {
                Product = product,
                Quantity = quantity,
            }
        }
            };

            _context.Purchases.Add(purchase);

            await _context.SaveChangesAsync();
        }

        public async Task SetSalePrice(int productId, decimal salePrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            product.SalePrice = salePrice;

            await _context.SaveChangesAsync();

        }

     
    }
}
