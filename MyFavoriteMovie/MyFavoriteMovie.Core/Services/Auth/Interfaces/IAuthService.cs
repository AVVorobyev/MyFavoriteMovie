using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.Core.Services.Interfaces
{
    public interface IAuthService
    {
        public AuthData GetAuthData(User user);
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string password);
    }
}
