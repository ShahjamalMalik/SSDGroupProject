using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GroupProjectDeployment.Models
{
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser() 
        {
            Cart = new List<ShoppingCart>();
        }    

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        public List<ShoppingCart>? Cart { get; set; }
    }
}
