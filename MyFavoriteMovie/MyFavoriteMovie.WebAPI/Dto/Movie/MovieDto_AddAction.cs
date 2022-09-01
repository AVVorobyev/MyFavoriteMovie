using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Movie
{
    public class MovieDto_AddAction
    {
        public string? Name { get; set; }
        public DateTime? RealeseDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Title { get; set; }
        public IFormFile? PosterFile { get; set; }

        public List<MovieImage> Images { get; set; } = new();
        public List<Core.Models.Actor> DirectedBy { get; set; } = new();
        public List<Core.Models.Actor> Actors { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<MovieAward> Awards { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
        public List<Episode> Episodes { get; set; } = new();
        public List<MovieRate> MovieRates { get; set; } = new();
        public List<Account> AccountsFavorite { get; set; } = new();
    }
}
