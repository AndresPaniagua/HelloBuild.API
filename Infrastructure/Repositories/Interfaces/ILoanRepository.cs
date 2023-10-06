using HelloBuild.Domain.Entities;

namespace HelloBuild.Infrastructure.Repositories.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Prestamo>
    {
        Task<bool> ValidateGuestUserLoan(string? userId);
    }
}
