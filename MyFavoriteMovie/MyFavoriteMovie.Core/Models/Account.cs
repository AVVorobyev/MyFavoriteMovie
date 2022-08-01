using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public List<Review> Reviews { get; set; } = new();
        public List<Movie> FavoriteMovies { get; set; } = new();
        public List<Actor> FavoriteActors { get; set; } = new();
        public List<MovieRate> MovieRates { get; set; } = new();
    }
}
