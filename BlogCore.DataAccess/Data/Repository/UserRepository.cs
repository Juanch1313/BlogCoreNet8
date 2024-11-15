using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void BlockUser(string Id)
        {
            var userFromDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == Id);

            if (userFromDb != null)
            {
                userFromDb.LockoutEnd = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void UnlockUser(string Id)
        {
            var userFromDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == Id);
            if (userFromDb != null) 
            {
                userFromDb.LockoutEnd = DateTime.Now;
                _context.SaveChanges();
            }
        }
    }
}
