using System.ComponentModel.DataAnnotations;

namespace buy_sell_project.Models
{
    public class SupplierProduct
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
