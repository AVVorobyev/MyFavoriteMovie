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
        public Task<DomainResult<T>> GetByIdAsync(Expression<Func<T, bool>> filter, string? includeProperties = null,
            bool asNoTracking = false);
        public Task<DomainResult<IEnumerable<T>>> GetAsync(int skip = 0, int take = 10, string? includeProperties = null);
        public Task<DomainResult> AddAsync(T entity);
        public Task<DomainResult> UpdateAsync(T entity);
        public Task<DomainResult> DeleteAsync(T entity);
    }
}
