using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Season { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? RealeseDate { get; set; }

        public Movie? Movie { get; set; }
    }
}
