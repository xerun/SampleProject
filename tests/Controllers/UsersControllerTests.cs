using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MyApi.Controllers;
using MyApi.Services;
using MyApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyApi.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;
        private readonly List<UserModel> _users;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();

            _users = new List<UserModel>
            {
                new UserModel { Id = 1, Name = "Alice", Email = "alice@example.ca" },
                new UserModel { Id = 2, Name = "Bob", Email = "bob@example.com" }
            };

            _controller = new UsersController(_mockUserService.Object);
        }

        [Fact]
        public void GetUsers_WithListOfUsers_ReturnsOkResult()
        {
            // Arrange
            _mockUserService.Setup(s => s.GetUsers()).Returns(_users);

            // Act
            var result = _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserModel>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public void GetUser_WithUser_WhenUserExists_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var user = _users.First(u => u.Id == userId);
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns(user);

            // Act
            var result = _controller.GetUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserModel>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public void GetUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var userId = 99; // Non-existing user
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns((UserModel)null);

            // Act
            var result = _controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
