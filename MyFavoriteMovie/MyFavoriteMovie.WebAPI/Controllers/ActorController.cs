using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Actor;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;
        private readonly ILogger<ActorController> _logger;

        public ActorController(ILogger<ActorController> logger, IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<JsonResult> IndexAsync(int? id)
        {
            var result
                = await _actorRepository.GetByIdAsync(m => m.Id == id,
                $"{nameof(Actor.Images)},{nameof(Actor.ActorsInMovie)}," +
                $"{nameof(Actor.DirectorsInMovie)},{nameof(Actor.Awards)}");

            _logger.LogInformation("", result.Message ?? "Ok");

            ActorDtoGet? actorDto = null;

            if (result.Success)
            {
                var actor = result.Result!;

                actorDto = new ActorDtoGet()
                {
                    Id = actor.Id,
                    Name = actor.Name,
                    Surname = actor.Surname,
                    Height = actor.Height,
                    BirthDate = actor.BirthDate,
                    DeathDate = actor.DeathDate,
                    AvatarImage = actor.AvatarImage,
                    ActorsInMovie = actor.ActorsInMovie,
                    DirectorsInMovie = actor.DirectorsInMovie,
                    Awards = actor.Awards
                };
            }

            return new JsonResult(actorDto);
        }

        [HttpGet]
        [ActionName("GetAll")]
        public async Task<JsonResult> GetAllAsync()
        {
            var result = await _actorRepository.GetAsync(take: 50);

            _logger.LogInformation("", result.Message ?? "Ok");

            List<ActorDtoGet> actorListDto = new();

            if (result.Success)
            {
                foreach (var actor in result.Result!)
                {
                    actorListDto.Add(new ActorDtoGet()
                    {
                        Id = actor.Id,
                        Name = actor.Name,
                        Surname = actor.Surname,
                        BirthDate = actor.BirthDate,
                        AvatarImage = actor.AvatarImage,
                    });
                }
            }

            return new JsonResult(actorListDto);
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<JsonResult> AddAsync(Actor actor)
        {
            var result = await _actorRepository.AddAsync(actor);

            _logger.LogInformation("", result.Message ?? "Ok");

            return new JsonResult(result.Message ?? "Successful!");
        }

        [HttpPut]
        [ActionName("Update")]
        public async Task<JsonResult> UpdateAsync(Actor actor)
        {
            var result = await _actorRepository.UpdateAsync(actor);

            _logger.LogInformation("", result.Message ?? "Ok");

            return new JsonResult(result.Message ?? "Successful!");
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<JsonResult> DeleteAsync(Actor actor)
        {
            var result = await _actorRepository.DeleteAsync(actor);

            _logger.LogInformation("", result.Message ?? "Ok");

            return new JsonResult(result.Message ?? "Successful!");
        }
    }
}
