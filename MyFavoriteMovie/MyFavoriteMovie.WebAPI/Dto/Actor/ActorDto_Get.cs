using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Actor
{
    public class ActorDto_Get
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public double? Height { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string? AvatarImage { get; set; }

        public List<ActorImage>? Images { get; set; }
        public List<Core.Models.Movie> ActorsInMovie { get; set; } = new();
        public List<Core.Models.Movie> DirectorsInMovie { get; set; } = new();
        public List<ActorAward> Awards { get; set; } = new();
    }
}
