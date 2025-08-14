// Models/Data/Repository/IRepository.cs
using System.Collections.Generic;

namespace Equinox.Models.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        int Count { get; }
        IEnumerable<T> List(QueryOptions<T> options);
        T? Get(int id);
        T? Get(string id);
        T? Get(QueryOptions<T> options);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
