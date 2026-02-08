using buy_sell_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace buy_sell_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("profit")]
        public async Task<IActionResult> GetProfit(DateTime startDate, DateTime endDate)
        {
            var profit = await _reportService.GetProfit(startDate, endDate);
            return Ok(profit);
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport()
        {
            var products = await _reportService.GetStockReport();
            return Ok(products);
        }

    }
}
