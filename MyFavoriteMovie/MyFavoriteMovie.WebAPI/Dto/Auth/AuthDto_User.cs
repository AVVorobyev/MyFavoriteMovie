using MyFavoriteMovie.Core.Models;

namespace MyFavoriteMovie.WebAPI.Dto.Auth
{
    public class AuthDto_User
    {
        public string? Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
