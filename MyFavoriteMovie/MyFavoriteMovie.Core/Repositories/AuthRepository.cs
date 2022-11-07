using MyFavoriteMovie.Core.Contexts;
using MyFavoriteMovie.Core.Models;
using MyFavoriteMovie.Core.Repositories.Interfaces;

namespace MyFavoriteMovie.Core.Repositories
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private DomainResult? _domainResult;

        public AuthRepository(MSSQLDbContext context) : base(context) { }

        public DomainResult<bool> IsEmailUnique(string? email)
        {
            try
            {
                var result = _context.Users.Any(u => u.Email == email);

                _domainResult = DomainResult<bool>.Succeeded(!result);
            }
            catch (Exception e)
            {
                _domainResult = DomainResult<bool>.Failed(e.Message + Environment.NewLine + e.InnerException);
            }

            return (DomainResult<bool>)_domainResult;
        }

        public DomainResult<bool> IsNicknameUnique(string? nickname)
        {
            try
            {
                var result = _context.Users.Any(u => u.Nickname == nickname);

                _domainResult = DomainResult<bool>.Succeeded(!result);
            }
            catch (Exception e)
            {
                _domainResult = DomainResult<bool>.Failed(e.Message + Environment.NewLine + e.InnerException);
            }

            return (DomainResult<bool>)_domainResult;
        }

        public DomainResult<bool> IsUserFirstInDB()
        {
            try
            {
                var result = _context.Users.Any();

                _domainResult = DomainResult<bool>.Succeeded(!result);
            }
            catch (Exception e)
            {
                _domainResult = DomainResult.Failed(e.Message + Environment.NewLine + e.InnerException);
            }

            return (DomainResult<bool>)_domainResult;
        }
    }
}
