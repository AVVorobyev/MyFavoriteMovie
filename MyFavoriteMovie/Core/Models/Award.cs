using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [NotMapped]
    public abstract class Award
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string? NominationName { get; set; }

        public AwardType AwardType { get; set; }
        public DateTime AwardDate { get; set; }
    }

    public enum AwardType
    {
        Winner,
        Nominee
    }
}
