using BlogCore.DataAccess.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            categoryRepository = new CategoryRepository(_context);
            articleRepository = new ArticleRepository(_context);
            sliderRepository = new SliderRepository(_context);
        }

        public ICategoryRepository categoryRepository { get; private set; }
        public IArticleRepository articleRepository { get; private set; }
        public ISliderRepository sliderRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
