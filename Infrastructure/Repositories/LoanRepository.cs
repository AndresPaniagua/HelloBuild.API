using HelloBuild.Domain.Entities;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using HelloBuild.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HelloBuild.Infrastructure.Repositories
{
    public class LoanRepository : BaseRepository<Prestamo>, ILoanRepository
    {
        public LoanRepository(PersistenceContext context)
            : base(context) { }

        public async Task<bool> ValidateGuestUserLoan(string? userId)
        {
            return _context.Prestamos != null && userId != null && await _context.Prestamos.AnyAsync((loan) => loan.UserId == userId);
        }
    }
}
