using System.ComponentModel.DataAnnotations;

namespace GroupProjectDeployment.Models
{
    public class ShoppingCartViewModel
    {
        public Guid CartId { get; set; }
        public string userId { get; set; }
        public Guid productId { get; set; }

        [Display(Name = "Product Name")]
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string? ImageUrl { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
}
