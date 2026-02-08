using buy_sell_project.Data;
using buy_sell_project.Models;
using Microsoft.EntityFrameworkCore;

namespace buy_sell_project.Services
{
    public class ReportService : IReportService
    {

        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetProfit(DateTime startDate, DateTime endDate)
        {
            var sales = await _context.Sales
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .ToListAsync();
            decimal profit = 0;

            foreach(var sale in sales)
            {
                foreach(var item in sale.Items)
                {
                    profit += (item.SalePrice - item.Product.PurchasePrice) * item.Quantity;
                }
            }
           
            return profit;
        }

        public async Task<List<Product>> GetStockReport()
        {
            return await _context.Products
                .OrderBy(p=>p.Name)
                .ToListAsync();
        }
    }
}
