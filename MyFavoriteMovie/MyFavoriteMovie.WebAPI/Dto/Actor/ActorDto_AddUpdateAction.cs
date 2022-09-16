using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Actor
{
    public class ActorDto_AddUpdateAction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string? Height { get; set; }
        public string? BirthDate { get; set; }
        public string? DeathDate { get; set; }
        public IFormFile? AvatarImage { get; set; }

        public List<ActorImage>? Images { get; set; }
        public List<Core.Models.Movie> ActorsInMovie { get; set; } = new();
        public List<Core.Models.Movie> DirectorsInMovie { get; set; } = new();
        public List<ActorAward> Awards { get; set; } = new();
    }
}
