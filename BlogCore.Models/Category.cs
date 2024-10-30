using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name of category")]
        [Required(ErrorMessage = "Enter a name for the category")]
        public string Name { get; set; }

        [Display(Name = "Order of visualization")]
        [Range(1, 100, ErrorMessage = "Value should be between 1 to 100")]
        public int? Order { get; set; }
    }
}
