using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Movie;
using MyFavoriteMovie.WebAPI.Utiles;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IWebHostEnvironment _environment;

        public MovieController(IMovieRepository movieRepository,
            IWebHostEnvironment environment)
        {
            _movieRepository = movieRepository;
            _environment = environment;
        }

        [HttpGet]
        [ActionName("Movie")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _movieRepository.GetByIdAsync(m => m.Id == id,
                    $"{nameof(Movie.DirectedBy)},{nameof(Movie.Actors)},{nameof(Movie.Reviews)}," +
                    $"{nameof(Movie.Awards)},{nameof(Movie.Genres)},{nameof(Movie.MovieRates)}");

                if (result.Success)
                {
                    string? poster = null;
                    string path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;
                    var movie = result.Result!;
                    double averageRate = GetAverageRate(movie);
                    MovieDto_MovieAction? movieDto = null;

                    if (movie.Poster != null)
                    {
                        if (System.IO.File.Exists(path + movie.Poster))
                        {
                            poster = $"{Request.Scheme}://{Request.Host}{Request.PathBase}" +
                                $"{WebConsts.MoviePosterDirectory}{movie.Poster}";
                        }
                    }

                    movieDto = new MovieDto_MovieAction()
                    {
                        Id = movie.Id,
                        Name = movie.Name,
                        RealeseDate = movie.RealeseDate,
                        Duration = movie.Duration,
                        Title = movie.Title,
                        Poster = poster,
                        DirectedBy = movie.DirectedBy,
                        Actors = movie.Actors,
                        Reviews = movie.Reviews,
                        Awards = movie.Awards,
                        Genres = movie.Genres,
                        AverageRate = averageRate
                    };

                    return Ok(movieDto);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpGet]
        [ActionName("Movies")]
        public async Task<IActionResult> GetAsync(int skip = 0, int take = 50)
        {           
            try
            {
                var result = await _movieRepository.GetAsync(skip, take,
                    includeProperties: $"{nameof(Movie.Actors)}," +
                    $"{nameof(Movie.Genres)},{nameof(Movie.MovieRates)}");

                if (result.Success)
                {
                    var movies = result.Result!;
                    double averageRate;
                    List<MovieDto_MoviesAction> movieListDto = new();

                    foreach (var movie in movies!)
                    {
                        averageRate = GetAverageRate(movie);

                        movieListDto.Add(new MovieDto_MoviesAction()
                        {
                            Id = movie.Id,
                            Name = movie.Name,
                            RealeseDate = movie.RealeseDate,
                            Duration = movie.Duration,
                            Poster = movie.Poster,
                            Actors = movie.Actors,
                            Genres = movie.Genres,
                            AverageRate = averageRate
                        });
                    }
                        return Ok(movieListDto);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> AddAsync([FromForm]MovieDto_AddUpdateAction movieDto)
        {
            if (movieDto == null) return BadRequest();

            try
            {
                string? posterName = null;

                if (movieDto.PosterFile != null)
                {
                    string paht = _environment.WebRootPath + WebConsts.MoviePosterDirectory;

                    posterName = await FileManager.SaveAsync(
                        movieDto.PosterFile,
                        paht);
                }

                var movie = new Movie()
                {
                    Name = movieDto.Name,
                    RealeseDate = new DateTime(),
                    Duration = movieDto.Duration,
                    Actors = movieDto.Actors,
                    Awards = movieDto.Awards,
                    Images = movieDto.Images,
                    Episodes = movieDto.Episodes,
                    Genres = movieDto.Genres,
                    Title = movieDto.Title,
                    Poster = posterName,
                    DirectedBy = movieDto.DirectedBy
                };

                var result = await _movieRepository.AddAsync(movie);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm]MovieDto_AddUpdateAction movieDto)
        {
            if (movieDto == null) return BadRequest();

            try
            {
                var result = await _movieRepository.GetByIdAsync(filter: m => m.Id == movieDto.Id, asNoTracking: true);

                if(result == null) return NotFound();

                string? newPoster = null;
                var path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;
                var oldPoster = result.Result!.Poster;

                if (oldPoster != null) FileManager.Delete(oldPoster, path);

                if(movieDto.PosterFile != null)
                    newPoster = await FileManager.SaveAsync(movieDto.PosterFile!, path);

                var movie = new Movie()
                {
                    Id = movieDto.Id,
                    Name = movieDto.Name,
                    RealeseDate = movieDto.RealeseDate,
                    Duration = movieDto.Duration,
                    Actors = movieDto.Actors,
                    Awards = movieDto.Awards,
                    Images = movieDto.Images,
                    Episodes = movieDto.Episodes,
                    Genres = movieDto.Genres,
                    Title = movieDto.Title,
                    Poster = newPoster,
                    DirectedBy = movieDto.DirectedBy
                };

                await _movieRepository.UpdateAsync(movie);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync([FromForm]Movie movie)
        {
            if (movie == null) return BadRequest();

            try
            {
                var path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;

                if (movie.Poster != null)
                    FileManager.Delete(movie.Poster, path);

                await _movieRepository.DeleteAsync(movie);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        private double GetAverageRate(Movie movie)
        {
            if (movie.MovieRates.Any())            
                return movie.MovieRates.Average(r => r.Rate);

            return 0;
        }
    }
}
