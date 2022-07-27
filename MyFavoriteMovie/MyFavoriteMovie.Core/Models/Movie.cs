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
        public DateTime? RealeseDate { get; set; }
        public TimeSpan? Duration { get; set; }

        [MinLength(1)]
        [MaxLength(1000)]
        public string? Title { get; set; }
        public string? Poster { get; set; }

        public List<MovieImage>? Images { get; set; }
        public List<Actor> DirectedBy { get; set; } = new List<Actor>();
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<MovieAward> Awards { get; set; } = new List<MovieAward>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        public List<MovieRate> MovieRates { get; set; } = new List<MovieRate>();
        public List<Account> AccountsFavorite { get; set; } = new List<Account>();
    }
}
