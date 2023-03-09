using CryptoHelper;
using Microsoft.IdentityModel.Tokens;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFavoriteMovie.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan;

        public AuthService(string jwtSecret, int jwtLifespan)
        {
            _jwtSecret = jwtSecret;
            _jwtLifespan = jwtLifespan;
        }
        public AuthData GetAuthData(User user)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(_jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("nickname", user.Nickname!),
                    new Claim(ClaimTypes.Role.ToString(), user.Role!)
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData()
            {
                Id = user.Id,
                Role = user.Role,
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds()
            };
        }

        public static int DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var decodedToken = tokenHandler.ReadJwtToken(token);

            string? idStr = decodedToken.Claims.FirstOrDefault(c =>
                c.Type == "id")?.Value;

            var success = int.TryParse(idStr, out int id);

            return success ? id : -1;
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, password);
        }
    }
}