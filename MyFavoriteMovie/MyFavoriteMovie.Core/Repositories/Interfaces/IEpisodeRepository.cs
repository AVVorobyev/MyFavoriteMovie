using MyFavoriteMovie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Repositories.Interfaces
{
    public interface IEpisodeRepository: IRepository<Episode>
    {
        public Task<DomainResult> AddRangeAsync(IEnumerable<Episode> episodes);
        public DomainResult<IEnumerable<Episode>> Get(int movieId);
    }
}
