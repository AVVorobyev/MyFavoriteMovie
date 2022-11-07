using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Services.Interfaces
{
    public interface IAuthService
    {
        public AuthData GetAuthData(int id);
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string password);
    }
}
