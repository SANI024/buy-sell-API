using buy_sell_project.Models;

namespace buy_sell_project.Services
{
    public interface IReportService
    {
        Task<decimal> GetProfit(DateTime startDate, DateTime endDate);
        Task<List<Product>> GetStockReport();
    }
}
