using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public List<Review> Reviews { get; set; } = new();
        public List<Movie> FavoriteMovies { get; set; } = new();
        public List<Actor> FavoriteActors { get; set; } = new();
        public List<MovieRate> MovieRates { get; set; } = new();
    }
}
