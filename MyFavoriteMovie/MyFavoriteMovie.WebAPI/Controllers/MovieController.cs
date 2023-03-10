using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto;
using MyFavoriteMovie.WebAPI.Dto.Movie;
using MyFavoriteMovie.WebAPI.Utiles;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieRepository movieRepository,
            IActorRepository actorRepository,
            IWebHostEnvironment environment,
            ILogger<MovieController> logger)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Finds and returns movie by id from Database
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <returns>Returns DomainResult with found movie</returns>
        [HttpGet]
        [ActionName("Movie")]
        public async Task<IActionResult> GetAsync([FromQuery] int id)
        {
            try
            {
                var result = await _movieRepository.GetAsync(m => m.Id == id,
                    $"{nameof(Movie.DirectedBy)},{nameof(Movie.Actors)},{nameof(Movie.Reviews)}," +
                    $"{nameof(Movie.Awards)},{nameof(Movie.Genres)},{nameof(Movie.MovieRates)}");

                if (result.Success)
                {
                    if (result.Result == null)
                        return NotFound();

                    string? poster = null;
                    string path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;
                    var movie = result.Result;
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

                    return Ok(DomainResult<MovieDto_MovieAction>.Succeeded(movieDto));
                }

                throw new Exception(result.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Gets movie list from Database
        /// </summary>
        /// <param name="skip">Count of bypass elements</param>
        /// <param name="take">Count of taken elements</param>
        /// <returns>Returns DomainResult with movies list</returns>
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
                    var movies = moviesRresult.Result;

                    if (movies == null)
                        return NotFound();

                    double averageRate;
                    List<MovieDto_MoviesAction> movieListDto = new();

                    string? poster = null;
                    string path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;

                    foreach (var movie in movies)
                    {
                        averageRate = GetAverageRate(movie);

                        if (movie.Poster != null)
                        {
                            if (System.IO.File.Exists(path + movie.Poster))
                            {
                                poster = $"{Request.Scheme}://{Request.Host}{Request.PathBase}" +
                                    $"{WebConsts.MoviePosterDirectory}{movie.Poster}";
                            }
                        }

                        movieListDto.Add(new MovieDto_MoviesAction()
                        {
                            Id = movie.Id,
                            Name = movie.Name,
                            ReleaseDate = movie.ReleaseDate,
                            Duration = movie.Duration,
                            Poster = poster,
                            Actors = movie.Actors,
                            Genres = movie.Genres,
                            AverageRate = averageRate,
                            Episodes = movie.Episodes,
                        });
                    }

                    var count = countResult.Result;

                    return Ok(new Dto_ListWithCount<MovieDto_MoviesAction>(movieListDto, count));
                }

                var errorMessage = moviesRresult.Message + Environment.NewLine + countResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Add new movie to Database
        /// </summary>
        /// <param name="movieDto">New movie to add</param>
        /// <returns>DomainResult</returns>
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> AddAsync([FromForm] MovieDto_AddUpdateAction movieDto)
        {
            if (movieDto == null)
                return NotFound();

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

                var result = await _movieRepository.AddAsync(movie);

                if (result.Success)
                    return Ok(DomainResult.Succeeded());

                throw new Exception(result.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Update movie in Database
        /// </summary>
        /// <param name="movieDto">New properties to update movie</param>
        /// <returns>DomainResult</returns>
        [HttpPut]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm] MovieDto_AddUpdateAction movieDto)
        {
            if (movieDto == null)
                return NotFound();

            try
            {
                var movieResult = await _movieRepository.GetAsync(
                    filter: m => m.Id == movieDto.Id, asNoTracking: true);

                if (movieResult.Success)
                {
                    var oldMovie = movieResult.Result;

                    if (oldMovie == null)
                        return NotFound();

                    string? newPoster = null;
                    var path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;
                    var oldPoster = oldMovie.Poster;

                    if (oldPoster != null)
                        FileManager.Delete(oldPoster, path);

                    if (movieDto.PosterFile != null)
                        newPoster = await FileManager.SaveAsync(movieDto.PosterFile, path);

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

                    var result = await _movieRepository.UpdateAsync(movie);

                    if (result.Success)
                        return Ok(DomainResult.Succeeded());

                    throw new Exception(result.Message);
                }

                throw new Exception(movieResult.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Delete movie from Database by id
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <returns>DomainResult</returns>
        [HttpDelete]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] int? movieId)
        {
            if (movieId == null)
                return NotFound();

            try
            {
                var movieResult = await _movieRepository.GetAsync(
                    filter: m => m.Id == movieId, asNoTracking: true);

                if (movieResult.Success)
                {
                    var movie = movieResult.Result;

                    if (movie == null)
                        return NotFound();

                    var path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;

                    if (movie!.Poster != null)
                        FileManager.Delete(movie.Poster, path);

                    await _movieRepository.DeleteAsync(movie);

                    return Ok(DomainResult.Succeeded());
                }

                throw new Exception(movieResult.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Add actor link to actors list in movie
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <param name="actorId">Actor id</param>
        /// <returns>DomainResult</returns>
        [HttpPatch]
        [ActionName("AddActor")]
        public async Task<IActionResult> AddActorToMovieAsync([FromQuery] int? movieId, [FromForm] int? actorId)
        {
            if (movieId == null || actorId == null)
                return NotFound();

            try
            {
                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId,
                    includeProperties: $"{nameof(Movie.Actors)}");

                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId);

                if (movieResult.Success && actorResult.Success)
                {
                    var movie = movieResult.Result;
                    var actor = actorResult.Result;

                    if (movie == null || actor == null)
                        return NotFound();

                    movie.Actors.Add(actor);

                    var result = await _movieRepository.UpdateAsync(movie);

                    if (result.Success)
                        return Ok(DomainResult.Succeeded());

                    throw new Exception(result.Message);
                }

                var errorMessage = actorResult.Message + Environment.NewLine + actorResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Delete actor link from actors list in movie
        /// </summary>
        /// <param name="movieId">Movie id</param>
        /// <param name="actorId">Actor id</param>
        /// <returns>DomainResult</returns>
        [HttpPatch]
        [ActionName("DeleteActor")]
        public async Task<IActionResult> DeleteActorFromMovieAsync(int? movieId, [FromForm] int? actorId)
        {
            if (movieId == null || actorId == null)
                return NotFound();

            try
            {
                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId,
                    includeProperties: $"{nameof(Movie.Actors)}");

                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId);

                if (movieResult.Success && actorResult.Success)
                {
                    var movie = movieResult.Result;
                    var actor = actorResult.Result;

                    if (movie == null || actor == null)
                        return NotFound();

                    movie.Actors.Remove(actor);

                    var result = await _movieRepository.UpdateAsync(movie);

                    if (result.Success)
                        return Ok(DomainResult.Succeeded());

                    throw new Exception(result.Message);
                }

                var errorMessage = actorResult.Message + Environment.NewLine + actorResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Returns movies list after filter by name property
        /// </summary>
        /// <param name="filter">Filter string</param>
        /// <param name="skip">Count of bypass elements</param>
        /// <param name="take">Count of taken elements</param>
        /// <returns>Return filtered movies list in DomainResult</returns>
        [HttpGet]
        [ActionName("filter_name")]
        public async Task<IActionResult> GetRangeBy_Name(string? filter, int skip = 0, int take = 10)
        {
            try
            {
                if (filter == null)
                    return Ok(DomainResult<Dto_ListWithCount<MovieDto_MoviesAction>>.Succeeded(
                       new Dto_ListWithCount<MovieDto_MoviesAction>(null, 0)));

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

                    if (movies == null)
                        return Ok(DomainResult<Dto_ListWithCount<MovieDto_MoviesAction>>.Succeeded(
                           new Dto_ListWithCount<MovieDto_MoviesAction>(null, 0)));

                    List<MovieDto_MoviesAction> moviesDto = new();

                    double averageRate;

                    foreach (var movie in movies)
                    {
                        averageRate = GetAverageRate(movie);

                        moviesDto.Add(new MovieDto_MoviesAction()
                        {
                            Id = movie.Id,
                            Name = movie.Name,
                            ReleaseDate = movie.ReleaseDate,
                            Duration = movie.Duration,
                            Poster = GetPoster(movie),
                            Actors = movie.Actors,
                            Genres = movie.Genres,
                            AverageRate = averageRate,
                            Episodes = movie.Episodes
                        });
                    }

                    return Ok(DomainResult<Dto_ListWithCount<MovieDto_MoviesAction>>.Succeeded(
                        new Dto_ListWithCount<MovieDto_MoviesAction>(moviesDto, count)));
                }

                var errorMessage = actorResult.Message + Environment.NewLine + countResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Returns average rate value in movie movieRate propety
        /// </summary>
        /// <param name="movie">Movie</param>
        /// <returns>Average rate</returns>
        [NonAction]
        private double GetAverageRate(Movie movie)
        {
            if (movie.MovieRates.Any())
                return movie.MovieRates.Average(r => r.Rate);

            return 0;
        }

        /// <summary>
        /// Returns blob of movie Poster
        /// </summary>
        /// <param name="movie">Movie</param>
        /// <returns>Returns blob of movie Poster.
        /// If movie not contains the poster, then return null</returns>
        [NonAction]
        private string? GetPoster(Movie movie)
        {
            string path = _environment.WebRootPath + WebConsts.MoviePosterDirectory;

            if (System.IO.File.Exists(path + movie.Poster))
            {
                return $"{Request.Scheme}://{Request.Host}{Request.PathBase}" +
                    $"{WebConsts.MoviePosterDirectory}{movie.Poster}";
            }

            return null;
        }
    }
}
