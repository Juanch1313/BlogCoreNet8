using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Article article)
        {
            var objFromDb = _context.Articles.FirstOrDefault(c => c.Id == article.Id);
            if (objFromDb != null) 
            {
                objFromDb.Name = article.Name;
                objFromDb.Description = article.Description;
                objFromDb.ImageUrl = article.ImageUrl;
                objFromDb.CategoryId = article.CategoryId;


                _context.SaveChanges();
            }
        }
    }
}
