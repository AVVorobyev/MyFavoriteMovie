using MyFavoriteMovie.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string? Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public TimeSpan? Duration { get; set; }

        [MinLength(1)]
        [MaxLength(1000)]
        public string? Description { get; set; }
        public string? Poster { get; set; }

        public List<MovieImage> Images { get; set; } = new();
        public List<Actor> DirectedBy { get; set; } = new();
        public List<Actor> Actors { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<MovieAward> Awards { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();
        public List<Episode> Episodes { get; set; } = new();
        public List<MovieRate> MovieRates { get; set; } = new();
        public List<Account> AccountsFavorite { get; set; } = new();
    }
}
