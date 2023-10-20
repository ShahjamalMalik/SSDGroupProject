using System.ComponentModel.DataAnnotations;

namespace GroupProjectDeployment.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(250, MinimumLength = 1)]
        [Required]
        public string ReviewText { get; set; }
        [Range(1,10)]
        [Required]
        public int StarAmount { get; set; }

        public Product? Product { get; set; }
        
        public Guid ProductId { get; set; }
    }
}
