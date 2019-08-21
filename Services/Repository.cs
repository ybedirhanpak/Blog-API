using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blog_Project.Settings;

namespace Blog_Project.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BlogDbContext _dbContext;

        public Repository(BlogDbContext dbContext)
        {
            this._dbContext = dbContext;
            this.Table = dbContext.Set<T>();
        }

        //Reference object to the table
        public DbSet<T> Table { get; set; }

        /**
         * Returns all of the table
         */
        public IQueryable<T> All()
        {
            return Table;
        }

        /**
         * Returns the entity with given id.
         * Returns null if no such entity
         */
        public T GetById(Guid id)
        {
            return Table.Find(id);
        }

        /**
         * Adds given entity to the table.
         * Returns true if added successfully.
         */
        public bool Add(T entity)
        {
            Table.Add(entity);
            return Save();
        }

        public bool Update(T entity)
        {
            Table.Update(entity);
            return Save();
        }


        public bool Delete(T entity)
        {
            Table.Remove(entity);
            return Save();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return Table.Where(where);
        }

        public IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc=false)
        {
            return (isDesc) ? Table.OrderByDescending(orderBy) : Table.OrderBy(orderBy);
        }

        private bool Save()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                // TODO: Log Exceptions
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
