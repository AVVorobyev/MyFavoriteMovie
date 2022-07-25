using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
        public List<Actor> FavoriteActors { get; set; } = new List<Actor>();
        public List<MovieRate> MovieRates { get; set; } = new List<MovieRate>();
    }
}
