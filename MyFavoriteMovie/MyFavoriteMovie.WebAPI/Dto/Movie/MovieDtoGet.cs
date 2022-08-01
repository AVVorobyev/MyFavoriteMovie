using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Movie
{
    public class MovieDtoGet
    {
        public int Id {get; set; }
        public string? Name {get; set; }
        public DateTime? RealeseDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Poster { get; set; }
        public List<Actor>? Actors { get; set; } = new();
        public List<Genre>? Genres { get; set; } = new();
        public double AverageRate { get; set; }
    }
}
