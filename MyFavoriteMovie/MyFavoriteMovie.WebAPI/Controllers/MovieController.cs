using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Movie;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger _logger;

        public MovieController(ILogger<MovieController> logger, IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<JsonResult> IndexAsync(int? id)
        {
            var result = await _movieRepository.GetByIdAsync(m => m.Id == id,
                $"{nameof(Movie.DirectedBy)},{nameof(Movie.Actors)},{nameof(Movie.Reviews)}," +
                $"{nameof(Movie.Awards)},{nameof(Movie.Genres)},{nameof(Movie.MovieRates)}");

            _logger.LogInformation("", result.Message ?? "Ok");

            MovieDtoIndex? movieDto = null;

            if (result.Success)
            {
                var movie = result.Result!;
                double averageRate = 0;

                averageRate = GetAverageRate(movie);
                
                movieDto = new MovieDtoIndex()
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    RealeseDate = movie.RealeseDate,
                    Duration = movie.Duration,
                    Title = movie.Title,
                    Poster = movie.Poster,
                    DirectedBy = movie.DirectedBy,
                    Actors = movie.Actors,
                    Reviews = movie.Reviews,
                    Awards = movie.Awards,
                    Genres = movie.Genres,
                    AverageRate = averageRate
                };
            }

            return new JsonResult(movieDto);
        }

        [HttpGet]
        [ActionName("GetAll")]
        public async Task<JsonResult> GetAllAsync()
        {
            var result = await _movieRepository.GetAsync(take: 50,
                includeProperties: $"{nameof(Movie.Actors)}," +
                $"{nameof(Movie.Genres)},{nameof(Movie.MovieRates)}");

            _logger.LogInformation("", result.Message ?? "Ok");

            List<MovieDtoGet> movieDto = new();

            if (result.Success)
            {
                movieDto = new();
                var movies = result.Result!;
                double averageRate;

                foreach (var movie in movies!)
                {
                    averageRate = GetAverageRate(movie);

                    movieDto.Add(new MovieDtoGet()
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
            }

            return new JsonResult(movieDto);
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<JsonResult> AddAsync(Movie movie)
        {
            var result = await _movieRepository.AddAsync(movie);

            _logger.LogInformation("", result.Message ?? "Ok");

            return new JsonResult(result.Message ?? "Successful!");
        }

        [HttpPut]
        [ActionName("Update")]
        public async Task<JsonResult> UpdateAsync(Movie movie)
        {
            var result = await _movieRepository.UpdateAsync(movie);

            _logger.LogInformation("", result.Message ?? "Ok");

            return new JsonResult(result.Message ?? "Successful!");
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<JsonResult> DeleteAsync(Movie movie)
        {
            var result = await _movieRepository.DeleteAsync(movie);

            _logger.LogInformation("", result.Message ?? "Ok");

            return new JsonResult(result.Message ?? "Successful!");
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
