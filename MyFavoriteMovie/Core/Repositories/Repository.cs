using Core.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MSSQLDbContext _context;
        private DomainResult? _domainResult;
        internal DbSet<T> _dbSet;

        public Repository(MSSQLDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<DomainResult> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Succeeded();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message);
            }

            return _domainResult;
        }

        public async Task<DomainResult> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Succeeded();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message);
            }

            return _domainResult;
        }

        public async Task<DomainResult<IEnumerable<T>>> GetAsync(int skip, int take)
        {
            try
            {
                var entity = await _dbSet.Skip(skip).Take(take).ToListAsync();

                _domainResult = DomainResult<IEnumerable<T>>.Succeeded(entity);
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message);
            }

            return (DomainResult<IEnumerable<T>>)_domainResult;
        }

        public async Task<DomainResult<T>> GetByIdAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includeProperties != null)
                {
                    foreach (var property in includeProperties.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
                }

                _domainResult = DomainResult<T>.Succeeded(await query.SingleAsync());
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message);
            }

            return (DomainResult<T>)_domainResult;
        }

        public async Task<DomainResult> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Succeeded();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message);
            }

            return _domainResult;
        }
    }
}
