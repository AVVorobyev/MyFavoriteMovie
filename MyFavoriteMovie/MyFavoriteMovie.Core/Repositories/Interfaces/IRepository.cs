using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<DomainResult<T>> GetAsync(Expression<Func<T, bool>> filter,
            string? includeProperties = null,
            bool asNoTracking = false);
        public Task<DomainResult<IEnumerable<T>>> GetRangeAsync(
            Expression<Func<T, bool>>? filter = null,
            int skip = 0,
            int take = 10,
            string? includeProperties = null);
        public Task<DomainResult> AddAsync(T entity);
        public Task<DomainResult> UpdateAsync(T entity);
        public Task<DomainResult> DeleteAsync(T entity);
        public Task<DomainResult<int>> GetCountAsync();
    }
}
