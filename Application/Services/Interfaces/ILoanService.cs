using HelloBuild.Domain.Models;

namespace HelloBuild.Application.Services.Interfaces
{
    public interface ILoanService
    {
        Task<Response> Add(LoanRequest request);

        Task<Response> Get(Guid loanId);
    }
}
