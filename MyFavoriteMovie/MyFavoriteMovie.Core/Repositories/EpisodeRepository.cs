using MyFavoriteMovie.Core.Contexts;
using MyFavoriteMovie.Core.Exceptions;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Repositories
{
    public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
    {
        private DomainResult? _domainResult;

        public EpisodeRepository(MSSQLDbContext context) : base(context)
        {
        }

        public async Task<DomainResult> AddRangeAsync(IEnumerable<Episode> episodes)
        {
            try
            {
                await _dbSet.AddRangeAsync(episodes);
                await _context.SaveChangesAsync();

                _domainResult = DomainResult.Succeeded();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message + 
                    $"{Environment.NewLine}" +
                    e.InnerException);
            }

            return _domainResult;
        }

        public DomainResult<IEnumerable<Episode>> Get(int movieId)
        {
            try
            {
                var episodes = _dbSet.Where(e => e.MovieId == movieId);

                if (episodes.Any())
                    _domainResult = DomainResult<IEnumerable<Episode>>.Succeeded(episodes);
                else
                    throw new NotFoundException();
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e +
                    $"{Environment.NewLine}" +
                    e.InnerException);
            }

            return (DomainResult<IEnumerable<Episode>>)_domainResult;
        }
    }
}
