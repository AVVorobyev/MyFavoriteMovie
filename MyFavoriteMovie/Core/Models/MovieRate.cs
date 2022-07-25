using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class MovieRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public byte Rate { get; set; }

        public Movie? Movie { get; set; }
        public Account? Account { get; set; }
    }
}
