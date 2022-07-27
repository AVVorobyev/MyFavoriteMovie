using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class MovieImage
    {
        [Key]
        public int Id { get; set; }
        public string? Image { get; set; }

        public Movie? Movie { get; set; }
    }
}
