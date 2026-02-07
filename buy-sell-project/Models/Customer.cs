namespace buy_sell_project.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}
