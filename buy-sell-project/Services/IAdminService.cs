using buy_sell_project.Models;
using buy_sell_project.DTO;
namespace buy_sell_project.Services
{
    public interface IAdminService
    {

        Task PurchaseFromSupplier(int supplierProductId, int quantity, decimal salePrice);
        Task SetSalePrice(int productId, decimal salePrice);
        Task ActivateProduct(int productId);
        Task DeactivateProduct(int productId);
        Task<List<AdminViewSupplierProd>> GetAllSupplierProducts();
        Task<List<Product>> GetAllProducts();

    }
}
