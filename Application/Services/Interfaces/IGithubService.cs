using HelloBuild.Domain.Models;

namespace HelloBuild.Application.Services.Interfaces
{
    public interface IGithubService
    {
        Task<(bool, List<GitRepositoryResponse>)> GetRepository(string username, string personalToken);
        Task<(bool, List<GitRepositoryResponse>)> GetFavoriteRepositories(string username, string personalToken);
    }
}
