using HelloBuild.Domain.Entities;
using HelloBuild.Infrastructure.Repositories;
using HelloBuild.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HelloBuild.Api.Tests.Infrastructure
{
    public class LoanRepositoryTest
    {
        [Fact]
        public async Task ValidateGuestUserLoan_True()
        {
            // Arrange
            DbContextOptions<PersistenceContext> options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseInMemoryDatabase(databaseName: "PruebaIngreso")
                .Options;

            IConfiguration config = new Mock<IConfiguration>().Object;

            PersistenceContext context = new(options, config);
            _ = context.Prestamos.Add(new Prestamo { Id = Guid.NewGuid(), UserId = "123" });
            _ = context.SaveChanges();

            LoanRepository loanRepository = new(context);

            // Act
            bool result = await loanRepository.ValidateGuestUserLoan("123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateGuestUserLoan_False()
        {
            // Arrange
            DbContextOptions<PersistenceContext> options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseInMemoryDatabase(databaseName: "PruebaIngreso")
                .Options;

            IConfiguration config = new Mock<IConfiguration>().Object;

            PersistenceContext context = new(options, config);
            LoanRepository loanRepository = new(context);

            // Act
            bool result = await loanRepository.ValidateGuestUserLoan("456");

            // Assert
            Assert.False(result);
        }
    }
}
