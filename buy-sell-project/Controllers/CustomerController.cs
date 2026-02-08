using buy_sell_project.DTO;
using buy_sell_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace buy_sell_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private ISaleService _saleservice;
        public CustomerController(ISaleService saleservice)
        {
            _saleservice = saleservice;
        }
        [HttpGet("products")]
        public async Task<IActionResult> GetAvailableProducts()
        {
            var products = await _saleservice.GetAvailableProducts();
            var dto = products.Select(p => new ProductCustomerDto
            {
                Name = p.Name,
                SalePrice = p.SalePrice,
                Quantity = p.Quantity,
                ProductId=p.Id
                
            }).ToList();

            return Ok(dto);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("buy")]
        public async Task<IActionResult> Buy(int productId, int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than 0");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _saleservice.CreateSale(productId, quantity, userId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
       
    }
}
