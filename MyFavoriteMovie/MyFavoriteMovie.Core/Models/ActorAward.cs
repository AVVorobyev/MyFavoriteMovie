using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Models
{
    public class ActorAward : Award
    {
        public List<Actor> ActorHolder { get; set; } = new List<Actor>();
    }
}
