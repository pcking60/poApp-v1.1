using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PostOfiice.DAta.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // marks an entity as new
        T Add(T entity);

        // marks an entity as modified
        void Update(T entity);

        // marks an entity as removed
        T Delete(T entity);

        T Delete(int id);

        void DeleteMulti(Expression<Func<T, bool>> where);

        T GetSingleByID(int id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IEnumerable<T> GetAll(string[] includes = null);

        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);
    }
}