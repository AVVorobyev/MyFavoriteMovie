using Microsoft.EntityFrameworkCore;
using MyFavoriteMovie.Core.Contexts;
using MyFavoriteMovie.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Repositories
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
                _domainResult = DomainResult.Failed(e.Message + 
                    $"{Environment.NewLine}" + e.InnerException);
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
                _domainResult = DomainResult.Failed(e.Message +
                    $"{Environment.NewLine}" + e.InnerException);
            }

            return _domainResult;
        }

        public async Task<DomainResult<IEnumerable<T>>> GetAsync(int skip = 0, int take = 10, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (includeProperties != null)
                {
                    foreach (var propery in includeProperties.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(propery);
                    }
                }

                var result = await query.Skip(skip).Take(take).ToListAsync();

                if (result != null && result.Any())
                    _domainResult = DomainResult<IEnumerable<T>>.Succeeded(result);
                else
                    throw new NotFoundException();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult<IEnumerable<T>>.Failed(e.Message +
                    $"{Environment.NewLine}" + e.InnerException);
            }

            return (DomainResult<IEnumerable<T>>)_domainResult;
        }

        public async Task<DomainResult<T>> GetByIdAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
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

                var result = await query.FirstOrDefaultAsync();

                if (result != null)
                    _domainResult = DomainResult<T>.Succeeded(result);
                else
                    throw new NotFoundException();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult<T>.Failed(e.Message +
                    $"{Environment.NewLine}" + e.InnerException);
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
