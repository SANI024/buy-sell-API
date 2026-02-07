namespace buy_sell_project.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
