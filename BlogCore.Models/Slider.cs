using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name of the slider")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public bool Status { get; set; }

        [Display(Name = "Creation Date")]
        public string? CreatedAt { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

    }
}
