using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFavoriteMovie.Core;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.Core.Services;
using MyFavoriteMovie.Core.Services.Auth;
using MyFavoriteMovie.Core.Services.Interfaces;
using MyFavoriteMovie.WebAPI.Dto.Auth;
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
        public async Task<IActionResult> RegistrationAsync([FromForm] AuthDto_RegistrationModel userDto)
        {
            try
            {
                if (userDto.Email == null)
                    return Ok(DomainResult.Failed("Email is null."));

                if (userDto.Nickname == null)
                    return Ok(DomainResult.Failed("Nickname is null."));

                if (userDto.Password == null)
                    return Ok(DomainResult.Failed("Password is null."));

                if (!_authRepository.IsNicknameUnique(userDto.Nickname).Result)
                    return Ok(DomainResult.Failed("User with such Nickname already exists"));

                if (!_authRepository.IsEmailUnique(userDto.Email).Result)
                    return Ok(DomainResult.Failed("User with such Email already exists."));

                var hashedPassword = _authService.HashPassword(userDto.Password);

                var isUserFirstInDBResult = _authRepository.IsUserFirstInDB();

                if (!isUserFirstInDBResult.Success)
                    return Ok(DomainResult.Failed(isUserFirstInDBResult.Message ?? "Unknown error."));
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
                    return Ok(DomainResult.Failed(addUserResult.Message ?? "Unknown error."));

                var getUserResult = await _authRepository.GetAsync(u => u.Nickname == userDto.Nickname);

                if (getUserResult.Success)
                    return Ok(DomainResult<AuthData>.Succeeded(_authService.GetAuthData(getUserResult.Result!)));

                return Ok(DomainResult.Failed(getUserResult.Message ?? "Unknown error."));
            }
            catch (Exception e)
            {
                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginAsync([FromForm] AuthDto_LogInModel userDto)
        {
            try
            {
                if (userDto.EmailNickname == null) return Ok(DomainResult.Failed("Email or Nickname is null."));
                if (userDto.Password == null) return Ok(DomainResult.Failed("Password or Nickname is null."));

                var getUserResult = await _authRepository.GetAsync(
                    u => u.Nickname == userDto.EmailNickname || u.Email == userDto.EmailNickname);

                if (getUserResult.Success)
                {
                    User? user = getUserResult.Result;

                    if (user == null || !_authService.VerifyPassword(user.Password!, userDto.Password))
                        return Ok(DomainResult.Failed("Invalid Email/Nickname or Password."));

                    var authData = _authService.GetAuthData(user);

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMilliseconds(authData.TokenExpirationTime),
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    };

                    HttpContext.Response.Cookies.Append(
                        "token",
                        authData.Token!,
                        cookieOptions
                        );

                    return Ok(DomainResult<AuthData>.Succeeded(authData));
                }

                return Ok(DomainResult.Failed(getUserResult.Message ?? "Unknown error."));
            }
            catch (Exception e)
            {
                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        [HttpGet]
        [ActionName("Nickname")]
        public IActionResult IsNicknameUnique(string nickname)
        {
            try
            {
                var result = _authRepository.IsNicknameUnique(nickname);

                if (result.Success)
                    return Ok(DomainResult<bool>.Succeeded(result.Result));

                return Ok(DomainResult.Failed(result.Message ?? "Unknown error."));
            }
            catch (Exception e)
            {
                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        [HttpGet]
        [ActionName("Email")]
        public IActionResult IsEmailUnique(string email)
        {
            try
            {
                var result = _authRepository.IsEmailUnique(email);

                if (result.Success)
                    return Ok(DomainResult<bool>.Succeeded(result.Result));

                return Ok(DomainResult.Failed(result.Message ?? "Unknown error."));
            }
            catch (Exception e)
            {
                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }

        [HttpGet]
        [Authorize]
        [ActionName("Profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            try
            {
                var token = Request.Cookies.FirstOrDefault(c =>
                    c.Key == "token").Value;

                if (token == null)
                    return Unauthorized();

                var id = AuthService.DecodeToken(token);

                if (id < 0)
                    return Unauthorized();

                var userResult = await _authRepository.GetAsync(u => u.Id == id);

                if (userResult.Success && userResult.Result != null)
                {
                    var userDto = new AuthDto_User()
                    {
                        Nickname = userResult.Result.Nickname,
                        Name = userResult.Result.Name,
                        Surname = userResult.Result.Surname,
                        Email = userResult.Result.Email,
                        RegistrationDate = userResult.Result.RegistrationDate,
                        Role = userResult.Result.Role
                    };

                    return Ok(DomainResult<AuthDto_User>.Succeeded(userDto));
                }
                else
                    return Ok(DomainResult.Failed(userResult.Message ?? "Unknown error."));
            }
            catch (Exception e)
            {
                return Ok(DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException));
            }
        }
    }
}