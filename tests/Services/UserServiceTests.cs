using Xunit;
using MyApi.Services;
using MyApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void GetUsers_ReturnsAllUsers()
        {
            // Act
            var result = _userService.GetUsers().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.Name == "Alice" && u.Email == "alice@example.ca");
            Assert.Contains(result, u => u.Name == "Bob" && u.Email == "bob@example.com");
        }

        [Fact]
        public void GetUserById_ReturnsCorrectUser_WhenUserExists()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Alice", result.Name);
        }

        [Fact]
        public void GetUserById_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 99; // Non-existing user

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }
    }
}
