using buy_sell_project.Models;
using BCrypt.Net;

namespace buy_sell_project.Data
{
    public class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
