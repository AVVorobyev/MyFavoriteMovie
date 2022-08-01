using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Home;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMovieRepository movieRepository, IActorRepository actorRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
        }

        public JsonResult Index()
        {
            List<MovieDto> movieDtos = new();
            var movieResult = _movieRepository.GetAsync().Result;

            _logger.LogInformation("", movieResult.Message ?? "Ok");

            if (movieResult.Success)
            {
                foreach (var movie in movieResult.Result!)
                {
                    movieDtos.Add(new MovieDto
                    {
                        Id = movie.Id,
                        Name = movie.Name,
                        Poster = movie.Poster,
                        RealeseDate = movie.RealeseDate
                    });
                }
            }

            List<ActorDto> actorDtos= new();
            var actorResult = _actorRepository.GetAsync().Result;

            _logger.LogInformation("", actorResult.Message ?? "Ok");

            if (actorResult.Success)
            {
                foreach (var actor in actorResult.Result!)
                {
                    actorDtos.Add(new ActorDto()
                    {
                        Id = actor.Id,
                        Name = actor.Name,
                        Surname = actor.Surname,
                        BirthDate = actor.BirthDate,
                        AvatarImage = actor.AvatarImage
                    });
                }
            }

            return new JsonResult(new { Movies = movieDtos, Actors = actorDtos });
        }
    }
}
