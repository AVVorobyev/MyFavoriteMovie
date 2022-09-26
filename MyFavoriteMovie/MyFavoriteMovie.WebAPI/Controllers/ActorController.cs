using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Actor;
using MyFavoriteMovie.WebAPI.Utiles;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;
        private readonly IWebHostEnvironment _environment;

        public ActorController(IActorRepository actorRepository, IWebHostEnvironment environment)
        {
            _actorRepository = actorRepository;
            _environment = environment;
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
                    string? avatarImage = null;

                    if(actor.AvatarImage != null)
                    {
                        var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;

                        if (System.IO.File.Exists(path + actor.AvatarImage))
                            avatarImage = $"{Request.Scheme}://{Request.Host}{Request.PathBase}" +
                                $"{WebConsts.ActorAvatarDirectory}{actor.AvatarImage}";
                    }

                    actorDto = new ActorDtoGet()
                    {
                        Id = actor.Id,
                        Name = actor.Name,
                        Surname = actor.Surname,
                        Height = actor.Height,
                        BirthDate = actor.BirthDate,
                        DeathDate = actor.DeathDate,
                        AvatarImage = avatarImage,
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
                var actorsResult = await _actorRepository.GetAsync(skip, take);
                var countResult = await _actorRepository.GetCountAsync();

                List<ActorDtoGet> actorListDto = new();

                if (actorsResult.Success && countResult.Success)
                {
                    foreach (var actor in actorsResult.Result!)
                    {
                        actorListDto.Add(new ActorDtoGet()
                        {
                            Id = actor.Id,
                            Name = actor.Name,
                            Surname = actor.Surname,
                            Height = actor.Height,
                            BirthDate = actor.BirthDate,
                            AvatarImage = actor.AvatarImage,
                            DeathDate = actor.DeathDate                            
                        });
                    }

                    var count = countResult.Result;

                    return Ok(new { Actors = actorListDto, Count = count } );
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
        public async Task<IActionResult> AddAsync([FromForm]ActorDto_AddUpdateAction actorDto)
        {
            if (actorDto == null) return BadRequest();

            try
            {
                string? avatarImage = null;

                if(actorDto.AvatarImage != null)
                {
                    var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;
                    avatarImage = await FileManager.SaveAsync(actorDto.AvatarImage, path);
                }
                                
                var actor = new Actor()
                {
                    Name = actorDto.Name,
                    Surname = actorDto.Surname,
                    Height = Parser.ParseToHeight(actorDto.Height),
                    BirthDate = Parser.ParseToDateTime(actorDto.BirthDate),
                    DeathDate = Parser.ParseToDateTime(actorDto.DeathDate),
                    AvatarImage = avatarImage,
                    DirectorsInMovie = actorDto.DirectorsInMovie,
                    ActorsInMovie = actorDto.ActorsInMovie,
                    Awards = actorDto.Awards,
                    Images = actorDto.Images
                };

                await _actorRepository.AddAsync(actor);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }           
        }

        [HttpPut]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm]ActorDto_AddUpdateAction actorDto)
        {
            if(actorDto == null) return BadRequest();

            try
            {
                var result = await _actorRepository.GetByIdAsync(filter: a => a.Id == actorDto.Id, asNoTracking: true);

                if (!result.Success) return NotFound();

                var oldAvatarImage = result.Result!.AvatarImage;
                var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;
                string? newAvatarImage = null;

                if (oldAvatarImage != null) FileManager.Delete(oldAvatarImage, path);

                if (actorDto.AvatarImage != null) 
                    newAvatarImage = await FileManager.SaveAsync(actorDto.AvatarImage, path);

                var actor = new Actor()
                {
                    Id = actorDto.Id,
                    Name = actorDto.Name,
                    Surname = actorDto.Surname,
                    Height = Parser.ParseToHeight(actorDto.Height),
                    BirthDate = Parser.ParseToDateTime(actorDto.BirthDate),
                    DeathDate = Parser.ParseToDateTime(actorDto.DeathDate),
                    AvatarImage = newAvatarImage,
                    ActorsInMovie = actorDto.ActorsInMovie,
                    DirectorsInMovie = actorDto.DirectorsInMovie,
                    Awards = actorDto.Awards
                };

                await _actorRepository.UpdateAsync(actor);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync([FromForm]Actor actor)
        {
            if (actor == null) return BadRequest();

            try
            {
                if(actor.AvatarImage != null)
                {
                    var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;
                    FileManager.Delete(actor.AvatarImage, path);
                }

                await _actorRepository.DeleteAsync(actor);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
