using buy_sell_project.Models;
using buy_sell_project.DTO;

namespace buy_sell_project.Services
{
    public interface ISaleService
    {
        Task<List<Product>> GetAvailableProducts();
        Task<(bool Success, string Message)> CreateSale(int productId, int quantity, int userId);
        
    }
}
