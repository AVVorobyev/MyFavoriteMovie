using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto;
using MyFavoriteMovie.WebAPI.Dto.Movie;
using MyFavoriteMovie.WebAPI.Utiles;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IWebHostEnvironment _environment;

        public MovieController(IMovieRepository movieRepository,
            IActorRepository actorRepository,
            IWebHostEnvironment environment)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _environment = environment;
        }

        [HttpGet]
        [ActionName("Movie")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var result = await _movieRepository.GetAsync(m => m.Id == id,
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
                        ReleaseDate = movie.ReleaseDate,
                        Duration = movie.Duration,
                        Description = movie.Description,
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
        public async Task<IActionResult> GetRangeAsync(int skip = 0, int take = 50)
        {           
            try
            {
                var moviesRresult = await _movieRepository.GetRangeAsync(null, skip, take,
                    includeProperties: $"{nameof(Movie.Actors)}," +
                    $"{nameof(Movie.Genres)},{nameof(Movie.MovieRates)},{nameof(Movie.Episodes)}");

                var countResult = await _movieRepository.GetCountAsync();

                if (moviesRresult.Success && countResult.Success)
                {
                    var movies = moviesRresult.Result!;
                    double averageRate;
                    List<MovieDto_MoviesAction> movieListDto = new();

                    foreach (var movie in movies!)
                    {
                        averageRate = GetAverageRate(movie);

                        movieListDto.Add(new MovieDto_MoviesAction()
                        {
                            Id = movie.Id,
                            Name = movie.Name,
                            ReleaseDate = movie.ReleaseDate,
                            Duration = movie.Duration,
                            Poster = movie.Poster,
                            Actors = movie.Actors,
                            Genres = movie.Genres,
                            AverageRate = averageRate,
                            Episodes = movie.Episodes,
                        });
                    }

                    var count = countResult.Result;

                    return Ok(new Dto_ListWithCount<MovieDto_MoviesAction>(movieListDto, count));
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
                    ReleaseDate = Parser.ParseToDateTime(movieDto.ReleaseDate),
                    Duration = Parser.ParseToTimeSpan(movieDto.Duration),
                    Actors = movieDto.Actors,
                    Awards = movieDto.Awards,
                    Images = movieDto.Images,
                    Episodes = movieDto.Episodes,
                    Genres = movieDto.Genres,
                    Description = movieDto.Description,
                    Poster = posterName,
                    DirectedBy = movieDto.DirectedBy
                };

                await _movieRepository.AddAsync(movie);

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
                var movieResult = await _movieRepository.GetAsync(filter: m => m.Id == movieDto.Id, asNoTracking: true);

                if(movieResult.Success)
                {
                    var oldMovie = movieResult.Result;

                    if (oldMovie != null) return NotFound();

                    string? newPoster = null;
                    var path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;
                    var oldPoster = oldMovie!.Poster;

                    if (oldPoster != null) FileManager.Delete(oldPoster, path);

                    if(movieDto.PosterFile != null)
                        newPoster = await FileManager.SaveAsync(movieDto.PosterFile!, path);

                    var movie = new Movie()
                    {
                        Id = movieDto.Id,
                        Name = movieDto.Name,
                        ReleaseDate = Parser.ParseToDateTime(movieDto.ReleaseDate),
                        Duration = Parser.ParseToTimeSpan(movieDto.Duration),
                        Actors = movieDto.Actors,
                        Awards = movieDto.Awards,
                        Images = movieDto.Images,
                        Episodes = movieDto.Episodes,
                        Genres = movieDto.Genres,
                        Description = movieDto.Description,
                        Poster = newPoster,
                        DirectedBy = movieDto.DirectedBy
                    };

                    await _movieRepository.UpdateAsync(movie);

                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery]int? movieId)
        {
            if (movieId == null) return BadRequest();

            try
            {
                var movieResult = await _movieRepository.GetAsync(filter: m => m.Id == movieId, asNoTracking: true);

                if (movieResult.Success)
                {
                    var movie = movieResult.Result;

                    if (movie == null)
                        return NotFound();

                    var path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;

                    if (movie!.Poster != null)
                        FileManager.Delete(movie.Poster, path);

                    await _movieRepository.DeleteAsync(movie);

                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpPatch]
        [ActionName("AddActor")]
        public async Task<IActionResult> AddActorToMovieAsync(int? movieId, [FromForm]int? actorId)
        {
            if (movieId == null || actorId == null) return BadRequest();

            try
            {
                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId,
                    includeProperties: $"{nameof(Movie.Actors)}");

                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId);

                if (movieResult.Success && actorResult.Success)
                {
                    var movie = movieResult.Result;
                    var actor = actorResult.Result;

                    if(movie != null && actor != null)
                        movie!.Actors.Add(actor);

                    await _movieRepository.UpdateAsync(movie!);

                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpPatch]
        [ActionName("DeleteActor")]
        public async Task<IActionResult> DeleteActorFromMovieAsync(int? movieId, [FromForm] int? actorId)
        {
            if (movieId == null || actorId == null) return BadRequest();

            try
            {
                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId,
                    includeProperties: $"{nameof(Movie.Actors)}");

                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId);

                if (movieResult.Success && actorResult.Success)
                {
                    var movie = movieResult.Result;
                    var actor = actorResult.Result;

                    if (movie != null && actor != null)
                        movie!.Actors.Remove(actor);

                    await _movieRepository.UpdateAsync(movie!);

                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }


        [HttpGet]
        [ActionName("filter_name")]
        public async Task<IActionResult> GetRangeBy_Name(string? filter, int skip = 0, int take = 10)
        {
            if (filter == null) return Ok(new Dto_ListWithCount<MovieDto_MoviesAction>(new List<MovieDto_MoviesAction>(), 0));

            try
            {
                filter = filter.TrimStart().TrimEnd();

                DomainResult<IEnumerable<Movie>>? actorResult = null;

                actorResult = await _movieRepository.GetRangeAsync(
                    a => a.Name!.Contains(filter!),
                    skip,
                    take,
                    $"{nameof(Movie.Actors)}");

                var countResult = await _actorRepository.GetCountAsync();

                if (actorResult.Success && countResult.Success)
                {
                    var movies = actorResult.Result;
                    var count = countResult.Result;

                    List<MovieDto_MoviesAction> moviesDto = new();

                    double averageRate;

                    foreach (var movie in movies!)
                    {
                        averageRate = GetAverageRate(movie);

                        moviesDto.Add(new MovieDto_MoviesAction()
                        {
                            Id = movie.Id,
                            Name = movie.Name,
                            ReleaseDate = movie.ReleaseDate,
                            Duration = movie.Duration,
                            Poster = movie.Poster,
                            Actors = movie.Actors,
                            Genres = movie.Genres,
                            AverageRate = averageRate,
                            Episodes = movie.Episodes
                        });                        
                    }

                    return Ok(new Dto_ListWithCount<MovieDto_MoviesAction>(moviesDto, count));
                }

            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
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
