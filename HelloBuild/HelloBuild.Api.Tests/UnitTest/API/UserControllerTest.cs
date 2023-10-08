using HelloBuild.Api.Controllers;
using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HelloBuild.Api.Tests.UnitTest.API
{
    public class UserControllerTest
    {
        [Theory]
        [InlineData(HttpStatusCode.OK, true, 200)]
        [InlineData(HttpStatusCode.BadRequest, false, 400)]
        public async Task SaveUser(HttpStatusCode status, bool success, int statusExpected)
        {
            // Arrange
            Mock<IUserService> userService = new();
            UserController controller = new(userService.Object);
            UserRequest request = new() { Name = "Name", Email = "valid@email.com", Password = "password" };

            _ = userService.Setup(u => u.AddUser(It.IsAny<UserRequest>())).ReturnsAsync(new Response { StatusCode = status, IsSuccess = success, Content = "" });

            // Act
            IActionResult result = await controller.Post(request);

            // Assert
            ObjectResult okResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(statusExpected, okResult.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, true, 200)]
        [InlineData(HttpStatusCode.NotFound, false, 404)]
        public async Task GetRegisteredUser(HttpStatusCode status, bool success, int statusExpected)
        {
            // Arrange
            Mock<IUserService> userService = new();
            UserController controller = new(userService.Object);
            UserExistRequest request = new() { Email = "email@email.com", Password = "password" };

            _ = userService.Setup(u => u.UserExist(It.IsAny<UserExistRequest>())).ReturnsAsync(new Response { StatusCode = status, IsSuccess = success, Content = "" });

            // Act
            IActionResult result = await controller.Get(request);

            // Assert
            ObjectResult okResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(statusExpected, okResult.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, true, 200)]
        [InlineData(HttpStatusCode.NotFound, false, 404)]
        public async Task GetUserInfo(HttpStatusCode status, bool success, int statusExpected)
        {
            // Arrange
            Mock<IUserService> userService = new();
            UserController controller = new(userService.Object);
            GetUserInfoRequest request = new() { Email = "email@email.com" };

            _ = userService.Setup(u => u.GetUserInfo(It.IsAny<GetUserInfoRequest>())).ReturnsAsync(new Response { StatusCode = status, IsSuccess = success, Content = "" });

            // Act
            IActionResult result = await controller.GetUserInfo(request);

            // Assert
            ObjectResult okResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(statusExpected, okResult.StatusCode);
        }

    }
}
