using AutoMapper;
using HelloBuild.Application.Services;
using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HelloBuild.Api.Tests.UnitTest.Application
{
    public class UserServiceTest
    {
        [Theory]
        [InlineData(HttpStatusCode.OK, true, 1)]
        [InlineData(HttpStatusCode.BadRequest, false, 0)]
        public async Task UserSave(HttpStatusCode statusExpected, bool success, int saved)
        {
            // Arrange
            Mock<IUserRepository> mockRepository = new();
            Mock<IMapper> mockMapper = new();
            UserService userService = new(mockRepository.Object, mockMapper.Object);

            UserRequest request = new()
            {
                Name = "foo",
                Email = "bar",
                Password = "baz"
            };

            _ = mockMapper.Setup(m => m.Map<User>(request)).Returns(new User());

            _ = mockRepository.Setup(r => r.AddAsync(It.IsAny<User>()));
            _ = mockRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(saved);

            // Act
            Response response = await userService.AddUser(request);

            // Assert
            Assert.Equal(statusExpected, response.StatusCode);
            Assert.Equal(success, response.IsSuccess);
            Assert.NotNull(response.Content);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, true, true)]
        [InlineData(HttpStatusCode.NotFound, false, false)]
        public async Task UserExist(HttpStatusCode statusExpected, bool success, bool repositoryExpect)
        {
            // Arrange
            Mock<IUserRepository> mockRepository = new();
            Mock<IMapper> mockMapper = new();
            UserService userService = new(mockRepository.Object, mockMapper.Object);

            UserExistRequest request = new()
            {
                Email = "bar",
                Password = "baz"
            };

            _ = mockRepository.Setup(r => r.FinduserEmailPasswordAsync(It.IsAny<UserExistRequest>())).ReturnsAsync(repositoryExpect);

            // Act
            Response response = await userService.UserExist(request);

            // Assert
            Assert.Equal(statusExpected, response.StatusCode);
            Assert.Equal(success, response.IsSuccess);
            Assert.NotNull(response.Content);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, true, true)]
        [InlineData(HttpStatusCode.NotFound, false, false)]
        public async Task GetUserInfo(HttpStatusCode statusExpected, bool success, bool repositoryExpect)
        {
            // Arrange
            Mock<IUserRepository> mockRepository = new();
            Mock<IMapper> mockMapper = new();
            UserService userService = new(mockRepository.Object, mockMapper.Object);

            GetUserInfoRequest request = new()
            {
                Email = "bar"
            };

            _ = mockRepository.Setup(r => r.FinduserEmailAsync(It.IsAny<string>())).ReturnsAsync(repositoryExpect ? new User() : null);

            // Act
            Response response = await userService.GetUserInfo(request);

            // Assert
            Assert.Equal(statusExpected, response.StatusCode);
            Assert.Equal(success, response.IsSuccess);
        }

    }
}
