using HelloBuild.Application.Helpers.Interfaces;
using HelloBuild.Application.Services;
using AutoMapper;
using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HelloBuild.Api.Tests.Application
{
    public class LoanServiceTest
    {
        [Theory]
        [InlineData("123", 3, true, false, HttpStatusCode.OK)]
        [InlineData("456", 1, true, false, HttpStatusCode.OK)]
        [InlineData("789", 3, false, true, HttpStatusCode.BadRequest)]
        public async Task LoanAdd(string identificacionUsuario, int tipoUsuario, bool success, bool hasLoans, HttpStatusCode code)
        {
            // Arrange
            Mock<ILoanRepository> mockLoanRepository = new();
            Mock<IValidateUserHelper> mockUserHelper = new();
            Mock<IMapper> mockMapper = new();

            LoanService loanService = new(mockLoanRepository.Object, mockMapper.Object, mockUserHelper.Object);

            LoanRequest request = new()
            {
                Isbn = Guid.NewGuid(),
                IdentificacionUsuario = identificacionUsuario,
                TipoUsuario = tipoUsuario
            };

            _ = mockUserHelper.Setup(u => u.ValidateGuestUserLoanAsync(request.IdentificacionUsuario))
                .ReturnsAsync(hasLoans); // Simula que el usuario tiene o no tiene préstamos

            _ = mockMapper.Setup(m => m.Map<Prestamo>(request))
                .Returns(new Prestamo()); // Simula la asignación de entidad

            _ = mockLoanRepository.Setup(r => r.AddAsync(It.IsAny<Prestamo>()));

            // Act
            Response response = await loanService.Add(request);

            // Assert
            Assert.Equal(code, response.StatusCode);
            Assert.Equal(success, response.IsSuccess);
            Assert.NotNull(response.Content);
        }

        [Theory]
        [InlineData("123", 3, true, true, HttpStatusCode.OK)]
        [InlineData("123", 3, false, false, HttpStatusCode.NotFound)]
        public async Task LoanGet(string userID, int userType, bool success, bool exists, HttpStatusCode code)
        {
            // Arrange
            Mock<ILoanRepository> mockLoanRepository = new();
            Mock<IMapper> mockMapper = new();

            LoanService loanService = new(mockLoanRepository.Object, mockMapper.Object, null);

            Guid loanId = Guid.NewGuid();

            Prestamo existingLoan = new()
            {
                Id = loanId,
                Isbn = Guid.NewGuid(),
                UserId = userID,
                UserType = userType,
                MaxDateReturn = DateTime.UtcNow,
            };

            _ = mockLoanRepository.Setup(r => r.GetByIdAsync(loanId))
                .ReturnsAsync(exists ? existingLoan : null);

            _ = mockMapper.Setup(m => m.Map<LoanUserResponse>(existingLoan))
                .Returns(new LoanUserResponse());

            // Act
            Response response = await loanService.Get(loanId);

            // Assert
            Assert.Equal(code, response.StatusCode);
            Assert.Equal(success, response.IsSuccess);
            Assert.NotNull(response.Content);
        }

    }
}
