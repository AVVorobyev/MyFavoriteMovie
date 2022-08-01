using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class MovieAward : Award
    {
        public List<Movie> MovieHolder { get; set; } = new();
    }
}
