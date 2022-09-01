using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Movie
{
    public class MovieDto_MoviesAction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? RealeseDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Poster { get; set; }
        public List<Core.Models.Actor>? Actors { get; set; } = new();
        public List<Genre>? Genres { get; set; } = new();
        public double AverageRate { get; set; }
    }
}
