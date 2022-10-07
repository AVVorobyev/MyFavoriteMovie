using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Episode;
using MyFavoriteMovie.WebAPI.Utiles;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly IEpisodeRepository _episodeRepository;

        public EpisodeController(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        [HttpGet]
        [ActionName("Episode")]
        public IActionResult Get([FromQuery]int? movieId)
        {
            if (movieId == null) return BadRequest();

            try
            {
                var result = _episodeRepository.Get((int)movieId);

                if (result.Success)
                {
                    var episodes = result.Result;

                    return Ok(episodes);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpPost]
        [ActionName("AddRange")]
        public async Task<IActionResult> AddRangeAsync([FromForm]IEnumerable<EpisodeDto_AddUpdateActions> episodesDto)
        {
            if (episodesDto == null) return BadRequest();

            try
            {
                var episodes = new List<Episode>();

                foreach (var item in episodesDto)
                {
                    episodes.Add(new Episode()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Season = item.Season,
                        RealeseDate = Parser.ParseToDateTime(item.RealeseDate),
                        Duration = Parser.ParseToTimeSpan(item.Duration),
                        MovieId = item.MovieId
                    });
                }

                await _episodeRepository.AddRangeAsync(episodes);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm]EpisodeDto_AddUpdateActions episodeDto)
        {
            if(episodeDto == null) return BadRequest();

            try
            {
                var episode = new Episode()
                {
                    Id = episodeDto.Id,
                    Name = episodeDto.Name,
                    Season = episodeDto.Season,
                    RealeseDate = Parser.ParseToDateTime(episodeDto.RealeseDate),
                    Duration = Parser.ParseToTimeSpan(episodeDto.Duration),
                    MovieId = episodeDto.MovieId
                };

                await _episodeRepository.UpdateAsync(episode);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery]int? episodeId)
        {
            if (episodeId == null) return BadRequest();

            try
            {
                var result = await _episodeRepository.GetAsync(e => e.Id == episodeId, asNoTracking: true);

                if (result.Success)
                {
                    var episode = result.Result;

                    if (episode == null) return NotFound();

                    await _episodeRepository.DeleteAsync(episode!);

                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }
    }
}
