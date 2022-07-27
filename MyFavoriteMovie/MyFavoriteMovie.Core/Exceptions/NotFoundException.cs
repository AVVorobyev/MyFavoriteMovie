using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public override string Message { get; }

        public NotFoundException() { Message = "NotFound"; }
        public NotFoundException(string message) : base(message) { Message = message; }
        public NotFoundException(string message, Exception inner) : base(message, inner) { Message = message; }

    }
}
