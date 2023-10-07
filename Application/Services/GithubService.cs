using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Octokit;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace HelloBuild.Application.Services
{
    public class GithubService : IGithubService
    {
        public async Task<(bool, List<GitRepositoryResponse>)> GetRepository(string username, string personalToken)
        {
            try
            {
                GitHubClient client = new(new ProductHeaderValue("HelloBuild"))
                {
                    Credentials = new Credentials(personalToken)
                };

                IReadOnlyList<Repository> repositories = await client.Repository.GetAllForUser(username);

                List<GitRepositoryResponse> repositoryNames = repositories.Select(repo => new GitRepositoryResponse { Name = repo.Name, Description = repo.Description }).ToList();

                return (true, repositoryNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new());
            }
        }

    }
}
