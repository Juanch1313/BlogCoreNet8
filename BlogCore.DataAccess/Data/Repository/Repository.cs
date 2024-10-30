using BlogCore.DataAccess.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext __context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            __context = context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string includeProperties = null)
        {
            //Creates a Iquerable consult from dbset context
            IQueryable<T> query = dbSet;

            //Implements the filter
            if(filter != null)
            {
                query = query.Where(filter);
            }

            //Includes properties of navegation
            if(includeProperties != null)
            {
                foreach (var includePropertie in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropertie);
                }
            }

            //Provides function of order
            if(OrderBy != null)
            {
                return OrderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            //Creates a Iquerable consult from dbset context
            IQueryable<T> query = dbSet;

            //Implements the filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //Includes properties of navegation
            if (includeProperties != null)
            {
                foreach (var includePropertie in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropertie);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
            dbSet.Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
