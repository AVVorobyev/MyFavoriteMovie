using MyFavoriteMovie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFavoriteMovie.Core.Repositories.Interfaces
{
    public interface IAuthRepository : IRepository<User>
    {
        public DomainResult<bool> IsEmailUnique(string? email);
        public DomainResult<bool> IsNicknameUnique(string? nickname);
        public DomainResult<bool> IsUserFirstInDB();
    }
}
