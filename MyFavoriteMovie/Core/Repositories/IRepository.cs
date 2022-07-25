using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<DomainResult<T>> GetByIdAsync(Expression<Func<T, bool>> filter, string? includeProperties);
        public Task<DomainResult<IEnumerable<T>>> GetAsync(int skip, int take);
        public Task<DomainResult> AddAsync(T entity);
        public Task<DomainResult> UpdateAsync(T entity);
        public Task<DomainResult> DeleteAsync(T entity);
    }
}
