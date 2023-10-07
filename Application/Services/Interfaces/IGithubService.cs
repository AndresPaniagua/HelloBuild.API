using HelloBuild.Domain.Models;

namespace HelloBuild.Application.Services.Interfaces
{
    public interface IGithubService
    {
        Task<(bool, List<GitRepositoryResponse>)> GetRepository(string username, string personalToken);
    }
}
