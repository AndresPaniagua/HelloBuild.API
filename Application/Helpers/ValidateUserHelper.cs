using HelloBuild.Application.Helpers.Interfaces;
using HelloBuild.Infrastructure.Repositories.Interfaces;

namespace HelloBuild.Application.Helpers
{
    public class ValidateUserHelper : IValidateUserHelper
    {
        private readonly ILoanRepository _repository;

        public ValidateUserHelper(ILoanRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ValidateGuestUserLoanAsync(string? userId)
        {
            return await _repository.ValidateGuestUserLoan(userId);
        }
    }
}
