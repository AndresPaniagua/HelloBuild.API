using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Context;
using HelloBuild.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HelloBuild.Api.Tests.UnitTest.Infrastructure
{
    public class UserRepositoryTest
    {
        [Fact]
        public async Task ValidateFindUserSuccess()
        {
            // Arrange
            DbContextOptions<PersistenceContext> options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseInMemoryDatabase(databaseName: "HelloBuildDB1")
                .Options;

            IConfiguration config = new Mock<IConfiguration>().Object;

            PersistenceContext context = new(options, config);
            _ = context.Users.Add(new User { Name = "Test", Email = "Test@email.com", Password = "validPassword" });
            _ = context.SaveChanges();

            UserRepository repository = new(context);

            UserExistRequest request = new()
            {
                Email = "Test@email.com",
                Password = "validPassword"
            };

            // Act
            bool result = await repository.FinduserEmailPasswordAsync(request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateFindUserError()
        {
            // Arrange
            DbContextOptions<PersistenceContext> options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseInMemoryDatabase(databaseName: "HelloBuildDB2")
                .Options;

            IConfiguration config = new Mock<IConfiguration>().Object;

            PersistenceContext context = new(options, config);
            UserRepository repository = new(context);

            UserExistRequest request = new()
            {
                Email = "Test@email.com",
                Password = "validPassword"
            };

            // Act
            bool result = await repository.FinduserEmailPasswordAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateFindUserByEmailSuccess()
        {
            // Arrange
            DbContextOptions<PersistenceContext> options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseInMemoryDatabase(databaseName: "HelloBuildDB3")
                .Options;

            IConfiguration config = new Mock<IConfiguration>().Object;

            PersistenceContext context = new(options, config);
            _ = context.Users.Add(new User { Name = "Test", Email = "Test@email.com", Password = "validPassword" });
            _ = context.SaveChanges();

            UserRepository repository = new(context);

            // Act
            User result = await repository.FinduserEmailAsync("Test@email.com");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ValidateFindUserByEmailError()
        {
            // Arrange
            DbContextOptions<PersistenceContext> options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseInMemoryDatabase(databaseName: "HelloBuildDB4")
                .Options;

            IConfiguration config = new Mock<IConfiguration>().Object;

            PersistenceContext context = new(options, config);
            UserRepository repository = new(context);

            // Act
            User result = await repository.FinduserEmailAsync("Test@email.com");

            // Assert
            Assert.Null(result);
        }

    }
}
