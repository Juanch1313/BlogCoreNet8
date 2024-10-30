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
        public int? Order { get; set; }
    }
}
