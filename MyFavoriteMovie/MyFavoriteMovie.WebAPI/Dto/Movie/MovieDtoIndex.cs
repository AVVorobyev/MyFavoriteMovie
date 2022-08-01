using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Movie
{
    public class MovieDtoIndex
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? RealeseDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Title { get; set; }
        public string? Poster { get; set; }
        public double AverageRate { get; set; }

        public List<Core.Models.Actor> DirectedBy { get; set; } = new();
        public List<Core.Models.Actor> Actors { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<MovieAward> Awards { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
    }
}
