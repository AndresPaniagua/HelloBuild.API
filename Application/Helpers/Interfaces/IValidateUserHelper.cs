namespace HelloBuild.Application.Helpers.Interfaces
{
    public interface IValidateUserHelper
    {
        Task<bool> ValidateGuestUserLoanAsync(string? userId);
    }
}
