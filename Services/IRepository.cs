using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog_Project.Services
{
    public interface IRepository<T> where T: class
    {
        DbSet<T> Table { get; }
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        T GetById(Guid id);

        IQueryable<T> All();
        IQueryable<T> Where(Expression<Func<T, bool>> where);
        IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);
        
    }
}
