using CryptoHelper;
using Microsoft.IdentityModel.Tokens;
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

        public AuthData GetAuthData(int userId)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(_jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name.ToString(), userId.ToString())
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
                Id = userId,
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds()
            };
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