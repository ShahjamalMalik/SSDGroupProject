using System.ComponentModel.DataAnnotations;

namespace GroupProjectDeployment.Models
{
    public class Checkout
    {
        public List<ShoppingCart> CartItems { get; set; }

        [Required]
        [Range(1, 999999999)]
        public int StudentNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public string PaymentMethod { get; set; }
    }
}
