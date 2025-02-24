using System.Collections.Generic;
using System.Linq;

namespace MyApi.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
    }
}
