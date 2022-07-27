using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string? Surname { get; set; }

        public double? HeightInMeters { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string? AvatarImage { get; set; }

        public List<ActorImage>? Images { get; set; }
        public List<Movie> ActorsInMovie { get; set; } = new List<Movie>();
        public List<Movie> DirectorsInMovie { get; set; } = new List<Movie>();
        public List<ActorAward> Awards { get; set; } = new List<ActorAward>();
        public List<Account> AccountsFavorite { get; set; } = new List<Account>();
    }
}
