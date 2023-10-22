using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupProjectDeployment.Models;

public class ShoppingCart
{
    [Key]
    public Guid CartId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    public Guid ProductId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    public string UserId { get; set; }

    public string userName { get; set; }
}
