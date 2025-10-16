using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T - Category
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();

        Task Add(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);


    }
}
