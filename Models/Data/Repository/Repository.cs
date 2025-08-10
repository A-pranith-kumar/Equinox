// Models/Data/Repository/Repository.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Equinox.Models.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected EquinoxContext Context { get; }
        private readonly DbSet<T> _dbset;

        public Repository(EquinoxContext ctx)
        {
            Context = ctx;
            _dbset = Context.Set<T>();
        }

        public int Count => _dbset.Count();

        public virtual IEnumerable<T> List(QueryOptions<T> options) => BuildQuery(options).ToList();

        public virtual T? Get(int id) => _dbset.Find(id);
        public virtual T? Get(string id) => _dbset.Find(id);

        public virtual T? Get(QueryOptions<T> options) => BuildQuery(options).FirstOrDefault();

        public virtual void Insert(T entity) => _dbset.Add(entity);
        public virtual void Update(T entity) => _dbset.Update(entity);
        public virtual void Delete(T entity) => _dbset.Remove(entity);
        public virtual void Save() => Context.SaveChanges();

        private IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = _dbset;

            foreach (var include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            if (options.HasWhere)
                query = query.Where(options.Where!);

            if (options.HasOrderBy)
            {
                query = options.OrderByDirection == "desc"
                    ? query.OrderByDescending(options.OrderBy!)
                    : query.OrderBy(options.OrderBy!);
            }

            if (options.HasPaging)
                query = query.PageBy(options.PageNumber, options.PageSize);

            return query;
        }
    }
}
