using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Actor;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;

        public ActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        [HttpGet]
        [ActionName("Actor")]
        public async Task<IActionResult> GetByIdAsync(int? id)
        {
            if (id == null) return BadRequest();

            try
            {
                var result = await _actorRepository.GetByIdAsync(m => m.Id == id,
                    $"{nameof(Actor.Images)},{nameof(Actor.ActorsInMovie)}," +
                    $"{nameof(Actor.DirectorsInMovie)},{nameof(Actor.Awards)}");

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

                    return Ok(actorDto);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpGet]
        [ActionName("Actors")]
        public async Task<IActionResult> GetAsync(int skip = 0, int take = 50)
        {
            try
            {
                var result = await _actorRepository.GetAsync(skip, take);

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

                    return Ok(actorListDto);
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
        public async Task<IActionResult> AddAsync(Actor actor)
        {
            var result = await _actorRepository.AddAsync(actor);

            return new JsonResult(result.Message ?? "Successful!");
        }

        [HttpPut]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateAsync(Actor actor)
        {
            var result = await _actorRepository.UpdateAsync(actor);

            return new JsonResult(result.Message ?? "Successful!");
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(Actor actor)
        {
            var result = await _actorRepository.DeleteAsync(actor);

            return new JsonResult(result.Message ?? "Successful!");
        }
    }
}
