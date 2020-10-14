using Domain.Base;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Base
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected IDbContext _db;
        protected readonly DbSet<T> _dbset;
        
        protected GenericRepository(IDbContext context)
        {
            _db = context;
            _dbset = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void AddRange(List<T> entities)
        {
            _dbset.AddRange(entities);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public void Edit(T entity)
        {
            _db.SetModified(entity);
        }

        public T Find(object id)
        {
            return _dbset.Find(id);
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool trackable = false)
        {
            return trackable ? _dbset.Where(predicate).AsEnumerable<T>() : _dbset.Where(predicate).AsNoTracking().AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool trackable = true)
        {
            IQueryable<T> query = _dbset;

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) return trackable ? orderBy(query) : orderBy(query).AsNoTracking();

            return trackable ? query.AsEnumerable<T>() : query.AsNoTracking().AsEnumerable<T>();
        }

        public T FindFirstOrDefault(Expression<Func<T, bool>> predicate, bool trackable = false)
        {
            return trackable ? _dbset.FirstOrDefault(predicate) : _dbset.AsNoTracking().FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.AsNoTracking().AsEnumerable<T>();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbset;

            if (predicate != null) query = query.Where(predicate);

            return query.Count();
        }

        public void Detach(T entity)
        {
            _db.Entry(entity).State = EntityState.Detached;
        }
    }
}
