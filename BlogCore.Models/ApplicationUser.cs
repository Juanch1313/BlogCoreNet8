using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "The address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The city is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "The country is required")]
        public string Country { get; set; }


    }
}
