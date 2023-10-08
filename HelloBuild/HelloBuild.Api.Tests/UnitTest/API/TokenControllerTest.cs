using HelloBuild.Api.Controllers;
using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HelloBuild.Api.Tests.UnitTest.API
{
    public class TokenControllerTests
    {
        [Fact]
        public async Task GetTokenSuccess()
        {
            // Arrange
            Mock<IConfiguration> configuration = new();
            Mock<IUserService> userService = new();
            TokenController controller = new(configuration.Object, userService.Object);
            TokenRequest validRequest = new() { Email = "valid@email.com", Password = "password" };

            _ = userService.Setup(u => u.UserExist(It.IsAny<UserExistRequest>())).ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK, IsSuccess = true });
            _ = configuration.Setup(u => u["Authentication:SecretKey"]).Returns("hfA2IfgOFRnDI+wj9Z7FDT6Y0jko3KlOnP3RdiLR1YfjxDSPFiwakQ==");
            _ = configuration.Setup(u => u["Authentication:Issuer"]).Returns("https://localhost:44358/");
            _ = configuration.Setup(u => u["Authentication:Audience"]).Returns("https://localhost:44358/");

            // Act
            IActionResult result = await controller.Authentication(validRequest);

            // Assert
            Assert.NotNull(result);
            _ = Assert.IsType<OkObjectResult>(result);

            OkObjectResult okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);

            dynamic tokenResponse = okResult.Value;
            Assert.NotNull(tokenResponse);
        }

        [Fact]
        public async Task GetTokenError()
        {
            // Arrange
            Mock<IConfiguration> configuration = new();
            Mock<IUserService> userService = new();
            TokenController controller = new(configuration.Object, userService.Object);
            TokenRequest validRequest = new() { Email = "invalid@email.com", Password = "Invalid password" };

            _ = userService.Setup(u => u.UserExist(It.IsAny<UserExistRequest>())).ReturnsAsync(new Response { StatusCode = HttpStatusCode.BadRequest, IsSuccess = false });

            // Act
            IActionResult result = await controller.Authentication(validRequest);

            // Assert
            Assert.NotNull(result);
            _ = Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult okResult = result as BadRequestObjectResult;
            Assert.Equal(400, okResult.StatusCode);
        }

    }
}
