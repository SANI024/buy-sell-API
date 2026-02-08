using buy_sell_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace buy_sell_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseFromSupplier(int supplierProductId, int quantity, decimal salePrice)
        {
            await _adminService.PurchaseFromSupplier(supplierProductId, quantity, salePrice);
            return Ok("Purchase completed");
        }

        [HttpPost("Update-sale-price")]
        public async Task<IActionResult> SetSalePrice(int productId, decimal salePrice)
        {
            await _adminService.SetSalePrice(productId, salePrice);
            return Ok("Sale price updated");
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateProduct(int productId)
        {
            await _adminService.ActivateProduct(productId);
            return Ok("Product activated");
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateProduct(int productId)
        {
            await _adminService.DeactivateProduct(productId);
            return Ok("Product deactivated");
        }

        [HttpGet("supplier-products")]
        public async Task<IActionResult> GetSupplierProducts()
        {
            var products = await _adminService.GetAllSupplierProducts();
            return Ok(products);
        }
        [HttpGet("boughted-products")]
        public async Task<IActionResult> GetAdminProducts()
        {
            var adminProds = await _adminService.GetAllProducts();
            return Ok(adminProds);
        }
    }
}
