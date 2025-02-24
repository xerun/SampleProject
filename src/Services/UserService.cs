using System.Collections.Generic;
using System.Linq;
using MyApi.Models;

namespace MyApi.Services
{
    public class UserService : IUserService
    {
        private readonly List<UserModel> _users = new()
        {
            new UserModel { Id = 1, Name = "Alice", Email = "alice@example.ca" },
            new UserModel { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };

        public IEnumerable<UserModel> GetUsers()
        {
            return _users;
        }

        public UserModel GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}
