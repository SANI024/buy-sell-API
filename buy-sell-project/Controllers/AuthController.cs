using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace buy_sell_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3)
                return BadRequest("Username must be at least 3 characters");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6 ||
                !dto.Password.Any(char.IsUpper) || !dto.Password.Any(char.IsDigit))
                return BadRequest("Password must be at least 6 characters, include one uppercase letter and one number");

            var validRoles = new[] { "Supplier", "Customer" };
            if (!validRoles.Contains(dto.Role))
                return BadRequest("Invalid role, role must be Supplier or Customer");

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            _context.Users.Add(user);

            if (dto.Role == "Supplier")
                _context.Suppliers.Add(new Supplier { User = user, CompanyName = "Supplier Company" });
            else if (dto.Role == "Customer")
                _context.Customers.Add(new Customer { User = user, FullName = user.Username });

            await _context.SaveChangesAsync();

            return Ok($"Registered as {dto.Role}");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Username or password incorrect");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "You are not authorized. Please login." });

            if (!int.TryParse(userIdClaim, out var userId))
                return BadRequest(new { message = "Invalid user identifier." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });


            if (user.Role == "Customer")
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
                if (customer != null) _context.Customers.Remove(customer);
            }
            else if (user.Role == "Supplier")
            {
                var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.UserId == userId);
                if (supplier != null) _context.Suppliers.Remove(supplier);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account deleted successfully." });
        }
    }
}