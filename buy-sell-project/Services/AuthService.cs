using buy_sell_project.Data;
using buy_sell_project.DTO;
using buy_sell_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace buy_sell_project.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)=> _context = context;

        public async Task<User> RegisterAsync(AuthDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new Exception("username alraedy exsists");
            var user = new User
            {
              Username = dto.Username,
              PasswordHash= dto.Password,
              Role= dto.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> LoginAsync(AuthDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) throw new Exception("please enter username");
            if (user.PasswordHash != dto.Password) throw new Exception("password is incorrect");
            return user;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
