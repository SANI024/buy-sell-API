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
    [Authorize(Roles = "Supplier")]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _service;

        public SupplierController(SupplierService service)
        {
            _service = service;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] SupplierProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "You are not authorized." });

            int userId = int.Parse(userIdClaim);

            await _service.AddProduct(userId, dto);

            return Ok(new { message = "Product added successfully" });
        }
    }
}
