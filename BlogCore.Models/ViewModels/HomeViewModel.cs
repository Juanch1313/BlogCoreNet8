using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Slider> SlidersList { get; set; }
        public IEnumerable<Article> ArticlesList { get; set; }
    }
}
