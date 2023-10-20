using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GroupProjectDeployment.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; }


    [Required]
    [StringLength(100)]
    public string Name { get; set; }


    [Required]
    [StringLength(150, MinimumLength = 10)]
    public string Description { get; set; }


    [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Please upload a valid image.")]
    public string? ImageUrl { get; set; }


    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price.")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }


    [Required]
    public int quantity { get; set; }

    
    public List<Review>? Reviews { get; set;}
}
