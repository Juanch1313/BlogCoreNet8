﻿using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var objFromDb = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (objFromDb != null) 
            {
                objFromDb.Name = category.Name;
                objFromDb.Order = category.Order;
                _context.SaveChanges();
            }
        }
    }
}