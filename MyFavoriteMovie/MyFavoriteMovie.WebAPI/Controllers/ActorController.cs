using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.Core.Services.Auth;
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
        private readonly ILogger<ActorController> _logger;

        public ActorController(
            IActorRepository actorRepository,
            IMovieRepository movieRepository,
            IWebHostEnvironment environment,
            ILogger<ActorController> logger)
        {
            _actorRepository = actorRepository;
            _environment = environment;
            _movieRepository = movieRepository;
            _logger = logger;
        }

        /// <summary>
        /// Finds and returns actor by id from Database
        /// </summary>
        /// <param name="id">Actor id</param>
        /// <returns>Returns DomainResult with found actor</returns>
        [HttpGet]
        [ActionName("Actor")]
        public async Task<IActionResult> GetAsync([FromQuery] int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var result = await _actorRepository.GetAsync(m => m.Id == id,
                    $"{nameof(Actor.Images)},{nameof(Actor.ActorsInMovie)}," +
                    $"{nameof(Actor.DirectorsInMovie)},{nameof(Actor.Awards)}");

                ActorDto_Get? actorDto = null;

                if (result.Success)
                {
                    if (result.Result == null)
                        return NotFound();

                    var actor = result.Result;
                    string? avatarImage = null;

                    if (actor.AvatarImage != null)
                    {
                        var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;

                        if (System.IO.File.Exists(path + actor.AvatarImage))
                            avatarImage = $"{Request.Scheme}://{Request.Host}{Request.PathBase}" +
                                $"{WebConsts.ActorAvatarDirectory}{actor.AvatarImage}";
                    }

                    actorDto = new ActorDto_Get()
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

                    return Ok(DomainResult<ActorDto_Get>.Succeeded(actorDto));
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
        /// Gets actors list from Database
        /// </summary>
        /// <param name="skip">Count of bypass elements</param>
        /// <param name="take">Count of taken elements</param>
        /// <returns>Returns DomainResult with actor list</returns>
        [HttpGet]
        [ActionName("Actors")]
        public async Task<IActionResult> GetRangeAsync([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            try
            {
                var actorsResult = await _actorRepository.GetRangeAsync(null, skip, take,
                    $"{nameof(Actor.ActorsInMovie)}");
                var countResult = await _actorRepository.GetCountAsync();

                List<ActorDto_Get> actorsDto = new();

                if (actorsResult.Success && countResult.Success)
                {
                    var actors = actorsResult.Result;
                    var count = countResult.Result;

                    if (actors == null)
                        return Ok(DomainResult<Dto_ListWithCount<ActorDto_Get>>.Succeeded(
                           new Dto_ListWithCount<ActorDto_Get>(null, 0)));

                    foreach (var actor in actors)
                    {
                        actorsDto.Add(new ActorDto_Get()
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

                    return Ok(DomainResult<Dto_ListWithCount<ActorDto_Get>>.Succeeded(
                           new Dto_ListWithCount<ActorDto_Get>(actorsDto, count)));
                }

                var errorMessage = actorsResult.Message + Environment.NewLine + countResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Add new actor to Database
        /// </summary>
        /// <param name="actorDto">New actor to add</param>
        /// <returns>DomainResult</returns>
        [HttpPost]
        [Authorize(Roles = $"{AuthRoles.ModeratorRole},{AuthRoles.AdministratorRole}")]
        [ActionName("Add")]
        public async Task<IActionResult> AddAsync([FromForm] ActorDto_AddUpdateAction actorDto)
        {
            if (actorDto == null)
                return NotFound();

            try
            {
                string? avatarImage = null;

                if (actorDto.AvatarImage != null)
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

                var result = await _actorRepository.AddAsync(actor);

                if (result.Success)
                    return Ok();

                throw new Exception(result.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Update acot in Database
        /// </summary>
        /// <param name="actorDto">New properties to update actor</param>
        /// <returns>DomainResult</returns>
        [HttpPut]
        [Authorize(Roles = $"{AuthRoles.ModeratorRole},{AuthRoles.AdministratorRole}")]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm] ActorDto_AddUpdateAction actorDto)
        {
            if (actorDto == null)
                return NotFound();

            try
            {
                var actorResult = await _actorRepository.GetAsync(
                    filter: a => a.Id == actorDto.Id, asNoTracking: true);

                if (actorResult.Success)
                {
                    if (actorResult.Result == null)
                        return NotFound();

                    var oldAvatarImage = actorResult.Result!.AvatarImage;
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

                    var result = await _actorRepository.UpdateAsync(actor);

                    if (result.Success)
                        return Ok();

                    throw new Exception(result.Message);
                }

                throw new Exception(actorResult.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Delete actor from Database by id
        /// </summary>
        /// <param name="actorId">Actor id</param>
        /// <returns>DomainResult</returns>
        [HttpDelete]
        [Authorize(Roles = $"{AuthRoles.ModeratorRole},{AuthRoles.AdministratorRole}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] int? actorId)
        {
            if (actorId == null)
                return NotFound();

            try
            {
                var actorResult = await _actorRepository.GetAsync(filter: a => a.Id == actorId,
                    asNoTracking: true);

                if (actorResult.Success)
                {
                    var actor = actorResult.Result;

                    if (actor == null)
                        return NotFound();


                    if (actor.AvatarImage != null)
                    {
                        var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;
                        FileManager.Delete(actor.AvatarImage, path);
                    }

                    var result = await _actorRepository.DeleteAsync(actor);

                    if (result.Success)
                        return Ok();

                    throw new Exception(result.Message);
                }

                throw new Exception(actorResult.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Returns actors list after filter by name property
        /// </summary>
        /// <param name="filter">Filter string</param>
        /// <param name="skip">Count of bypass elements</param>
        /// <param name="take">Count of taken elements</param>
        /// <returns>Return filtered actors list in DomainResult</returns>
        [HttpGet]
        [ActionName("filter_name_surname")]
        public async Task<IActionResult> GetRangeByNameSurname(
            [FromQuery] string? filter,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10)
        {
            try
            {
                if (filter == null)
                    return Ok(DomainResult<Dto_ListWithCount<ActorDto_Get>>.Succeeded(
                        new Dto_ListWithCount<ActorDto_Get>(null, 0)));

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

                    if (actors == null)
                        return Ok(DomainResult<Dto_ListWithCount<ActorDto_Get>>.Succeeded(
                           new Dto_ListWithCount<ActorDto_Get>(null, 0)));

                    List<ActorDto_Get> actorsDto = new();

                    foreach (var actor in actors)
                    {
                        actorsDto.Add(new ActorDto_Get()
                        {
                            Id = actor.Id,
                            Name = actor.Name,
                            Surname = actor.Surname,
                            Height = actor.Height,
                            BirthDate = actor.BirthDate,
                            AvatarImage = GetAvatar(actor),
                            DeathDate = actor.DeathDate,
                            ActorsInMovie = actor.ActorsInMovie
                        });
                    }

                    return Ok(DomainResult<Dto_ListWithCount<ActorDto_Get>>.Succeeded(
                        new Dto_ListWithCount<ActorDto_Get>(actorsDto, count)));
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
        /// Add movie link to movies list in movie
        /// </summary>
        /// <param name="actorId">Actor id</param>
        /// <param name="movieId">Movie id</param>
        /// <returns>DomainResult</returns>
        [HttpPatch]
        [Authorize(Roles = $"{AuthRoles.ModeratorRole},{AuthRoles.AdministratorRole}")]
        [ActionName("AddMovie")]
        public async Task<IActionResult> AddMovieToMovieAsync([FromQuery] int? actorId, [FromForm] int? movieId)
        {
            if (movieId == null || actorId == null)
                return NotFound();

            try
            {
                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId,
                    includeProperties: $"{nameof(Actor.ActorsInMovie)}");

                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId);

                if (actorResult.Success && movieResult.Success)
                {
                    var actor = actorResult.Result;
                    var movie = movieResult.Result;

                    if (actor == null || movie == null)
                        return NotFound();

                    actor.ActorsInMovie.Add(movie);

                    var result = await _actorRepository.UpdateAsync(actor);

                    if (result.Success)
                        return Ok(DomainResult.Succeeded());

                    throw new Exception(result.Message);
                }

                var errorMessage = actorResult.Message + Environment.NewLine + movieResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Delete movie link from movies list in movie
        /// </summary>
        /// <param name="actorId">Actor id</param>
        /// <param name="movieId">Movie id</param>
        /// <returns>DomainResult</returns>
        [HttpPatch]
        [Authorize(Roles = $"{AuthRoles.ModeratorRole},{AuthRoles.AdministratorRole}")]
        [ActionName("DeleteMovie")]
        public async Task<IActionResult> DeleteMovieFromMovieAsync([FromQuery] int? actorId, [FromForm] int? movieId)
        {
            if (actorId == null || movieId == null)
                return NotFound();

            try
            {
                var actorResult = await _actorRepository.GetAsync(a => a.Id == actorId,
                    includeProperties: $"{nameof(Actor.ActorsInMovie)}");

                var movieResult = await _movieRepository.GetAsync(m => m.Id == movieId);

                if (actorResult.Success && movieResult.Success)
                {
                    var actor = actorResult.Result;
                    var movie = movieResult.Result;

                    if (movie == null || actor == null)
                        return NotFound();

                    actor.ActorsInMovie.Remove(movie);

                    var result = await _actorRepository.UpdateAsync(actor);

                    return Ok(DomainResult.Succeeded());
                }

                var errorMessage = actorResult.Message + Environment.NewLine + movieResult.Message;

                throw new Exception(errorMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + Environment.NewLine + e.InnerException);

                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        /// <summary>
        /// Returns blob of actor Avatar
        /// </summary>
        /// <param name="actor">Actor</param>
        /// <returns>Returns blob of actor Avatar.
        /// If actor not contains the avatar, then return null</returns>
        private string? GetAvatar(Actor actor)
        {
            var path = _environment.WebRootPath + WebConsts.ActorAvatarDirectory;

            if (System.IO.File.Exists(path + actor.AvatarImage))
                return $"{Request.Scheme}://{Request.Host}{Request.PathBase}" +
                    $"{WebConsts.ActorAvatarDirectory}{actor.AvatarImage}";

            return null;
        }
    }
}
