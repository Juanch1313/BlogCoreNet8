using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _context;

        public SliderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Slider slider)
        {
            var objFromDb = _context.Sliders.FirstOrDefault(c => c.Id == slider.Id);
            if (objFromDb != null) 
            {
                objFromDb.Name = slider.Name;
                objFromDb.Status = slider.Status;
                objFromDb.ImageUrl = slider.ImageUrl;


                _context.SaveChanges();
            }
        }
    }
}
