using System.ComponentModel.DataAnnotations;

namespace buy_sell_project.DTO
{
    public class SupplierProductDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }


    }
}
