using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public string? Title { get; set; }

        public ReviewType ReviewType { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(3000)]
        public string? Text { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int AuthorId { get; set; }
        public Account? Author { get; set; }
    }

    public enum ReviewType
    {
        Neutral,
        Negative,
        Positive
    }
}
