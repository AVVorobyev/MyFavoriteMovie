using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.Core.Services.Auth;
using MyFavoriteMovie.Core.Services.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.User;
using MyFavoriteMovie.WebAPI.Utiles;

namespace MyFavoriteMovie.WebAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAuthService _authService;

        public AuthController(IAuthRepository userRepository, IAuthService authService)
        {
            _authRepository = userRepository;
            _authService = authService;
        }

        [HttpPost]
        [ActionName("Registration")]
        public async Task<IActionResult> RegistrationAsync([FromForm] UserDto_AuthModel userDto)
        {
            try
            {
                if (userDto.Email == null) return BadRequest("Email is null.");
                if (userDto.Nickname == null) return BadRequest("Nickname is null.");
                if (userDto.Password == null) return BadRequest("Password is null.");

                if (!_authRepository.IsNicknameUnique(userDto.Nickname).Result)
                    return BadRequest("User with such Nickname already exists.");

                if (!_authRepository.IsEmailUnique(userDto.Email).Result)
                    return BadRequest("User with such Email already exists.");

                var hashedPassword = _authService.HashPassword(userDto.Password);

                var isUserFirstInDBResult = _authRepository.IsUserFirstInDB();

                if (!isUserFirstInDBResult.Success)
                    return BadRequest(isUserFirstInDBResult.Message);

                string role = isUserFirstInDBResult.Result ? AuthRoles.AdministratorRole : AuthRoles.UserRole;

                User user = new()
                {
                    Email = userDto.Email,
                    Nickname = userDto.Nickname,
                    Password = hashedPassword,
                    RegistrationDate = UtcTime.Get(),
                    Role = role
                };

                var addUserResult = await _authRepository.AddAsync(user);

                if (!addUserResult.Success)
                    return BadRequest(addUserResult.Message);

                var getUserResult = await _authRepository.GetAsync(u => u.Nickname == userDto.Nickname);

                if (getUserResult.Success)
                    return Ok(_authService.GetAuthData(getUserResult.Result!.Id));

                return BadRequest(getUserResult.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + Environment.NewLine + e.InnerException);
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LogInAsync([FromForm] UserDto_AuthModel userDto)
        {
            try
            {
                if (userDto.Password == null) return BadRequest("Password is null.");

                var getUserResult = await _authRepository.GetAsync(
                    u => u.Nickname == userDto.Nickname || u.Email == userDto.Email);

                if (getUserResult.Success)
                {
                    User? user = getUserResult.Result;

                    if (user == null)
                        return NotFound();

                    if (!_authService.VerifyPassword(user.Password!, userDto.Password))
                        return BadRequest("Invalid password.");

                    return Ok(_authService.GetAuthData(user.Id));
                }

                return BadRequest(getUserResult.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + Environment.NewLine + e.InnerException);
            }
        }
    }
}
