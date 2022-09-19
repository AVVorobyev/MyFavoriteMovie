using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Movie
{
    public class MovieDto_AddUpdateAction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ReleaseDate { get; set; }
        public string? Duration { get; set; }
        public string? Description { get; set; }
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
