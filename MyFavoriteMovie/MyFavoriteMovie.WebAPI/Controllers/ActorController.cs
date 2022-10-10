using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.WebAPI.Dto;
using MyFavoriteMovie.WebAPI.Dto.Actor;
using MyFavoriteMovie.WebAPI.Utiles;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMovieRepository _movieRepository;

        public ActorController(IActorRepository actorRepository, IMovieRepository movieRepository, IWebHostEnvironment environment)
        {
            _actorRepository = actorRepository;
            _environment = environment;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [ActionName("Actor")]
        public async Task<IActionResult> GetAsync(int? id)
        {
            if (id == null) return BadRequest();

            try
            {
                var result = await _actorRepository.GetAsync(m => m.Id == id,
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
        public async Task<IActionResult> GetRangeAsync(int skip = 0, int take = 50)
        {   
            try
            {
                var actorsResult = await _actorRepository.GetRangeAsync(null, skip, take,
                    $"{nameof(Actor.ActorsInMovie)}");
                var countResult = await _actorRepository.GetCountAsync();

                List<ActorDtoGet> actorsDto = new();

                if (actorsResult.Success && countResult.Success)
                {
                    foreach (var actor in actorsResult.Result!)
                    {
                        actorsDto.Add(new ActorDtoGet()
                        {
                            Id = actor.Id,
                            Name = actor.Name,
                            Surname = actor.Surname,
                            Height = actor.Height,
                            BirthDate = actor.BirthDate,
                            AvatarImage = actor.AvatarImage,
                            DeathDate = actor.DeathDate,
                            ActorsInMovie = actor.ActorsInMovie
                        });
                    }

                    var count = countResult.Result;

                    return Ok(new Dto_ListWithCount<ActorDtoGet>(actorsDto, count));
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
                var result = await _actorRepository.GetAsync(filter: a => a.Id == actorDto.Id, asNoTracking: true);

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

        [HttpGet]
        [ActionName("filter_name_surname")]
        public async Task<IActionResult> GetRangeByNameSurname(string? filter, int skip = 0, int take = 10)
        {
            if(filter == null) return Ok(new Dto_ListWithCount<ActorDtoGet>(new List<ActorDtoGet>(), 0));

            try
            {
                filter = filter.TrimStart().TrimEnd();
                                
                var index = filter.IndexOf(' ');

                DomainResult<IEnumerable<Actor>>? actorResult = null;

                if (index > -1)
                {
                    var substring1 = filter.Substring(0, index);
                    substring1 = substring1.TrimStart().TrimEnd();

                    var substring2 = filter.Remove(0, index);
                    substring2 = substring2.TrimStart().TrimEnd();

                    actorResult = await _actorRepository.GetRangeAsync(
                        a => a.Name!.Contains(substring1) || a.Name.Contains(substring2)
                        || a.Surname!.Contains(substring1) || a.Surname.Contains(substring2),
                        skip,
                        take,
                        $"{nameof(Actor.ActorsInMovie)}");
                }
                else
                {
                    actorResult = await _actorRepository.GetRangeAsync(
                        a => a.Name!.Contains(filter!) || a.Surname!.Contains(filter!),
                        skip,
                        take,
                        $"{nameof(Actor.ActorsInMovie)}");
                }

                var countResult = await _actorRepository.GetCountAsync();

                if (actorResult.Success && countResult.Success)
                {
                    var actors = actorResult.Result;
                    var count = countResult.Result;

                    List<ActorDtoGet> actorsDto = new();

                    foreach (var actor in actors!)
                    {
                        actorsDto.Add(new ActorDtoGet()
                        {
                            Id = actor.Id,
                            Name = actor.Name,
                            Surname = actor.Surname,
                            Height = actor.Height,
                            BirthDate = actor.BirthDate,
                            AvatarImage = actor.AvatarImage,
                            DeathDate = actor.DeathDate,
                            ActorsInMovie = actor.ActorsInMovie
                        });
                    }

                    return Ok(new Dto_ListWithCount<ActorDtoGet>(actorsDto, count));
                }

            }
            catch (Exception)
            {
                throw;
            }

            return NotFound();
        }

        [HttpPatch]
        [ActionName("AddMovie")]
        public async Task<IActionResult> AddMovieToMovieAsync(int? actorId, [FromForm] int? movieId)
        {
            if (movieId == null || actorId == null) return BadRequest();

            try
            {
                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId,
                    includeProperties: $"{nameof(Actor.ActorsInMovie)}");

                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId);

                if (actorResult.Success && movieResult.Success)
                {
                    var actor = actorResult.Result;
                    var movie = movieResult.Result;

                    if (actor != null && movie != null)
                        actor!.ActorsInMovie.Add(movie);

                    await _actorRepository.UpdateAsync(actor!);

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
        [ActionName("DeleteMovie")]
        public async Task<IActionResult> DeleteMovieFromMovieAsync(int? actorId, [FromForm] int? movieId)
        {
            if (actorId == null || movieId == null) return BadRequest();

            try
            {
                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId,
                    includeProperties: $"{nameof(Actor.ActorsInMovie)}");

                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId);

                if (actorResult.Success && movieResult.Success)
                {
                    var actor = actorResult.Result;
                    var movie = movieResult.Result;

                    if (movie != null && actor != null)
                        actor!.ActorsInMovie.Remove(movie);

                    await _actorRepository.UpdateAsync(actor!);

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
