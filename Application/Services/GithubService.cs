using AutoMapper;
using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Newtonsoft.Json;
using Octokit;

namespace HelloBuild.Application.Services
{
    public class GithubService : IGithubService
    {
        private readonly IMapper _mapper;

        public GithubService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<(bool, List<GitRepositoryResponse>)> GetRepository(string username, string personalToken)
        {
            try
            {
                GitHubClient client = new(new ProductHeaderValue("HelloBuild"))
                {
                    Credentials = new Credentials(personalToken)
                };

                IReadOnlyList<Repository> repositories = await client.Repository.GetAllForUser(username);

                List<GitRepositoryResponse> repositoryNames = repositories.Select(repo => new GitRepositoryResponse
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    SvnUrl = repo.SvnUrl
                }).ToList();

                return (true, repositoryNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new());
            }
        }

        public async Task<(bool, List<GitRepositoryResponse>)> GetFavoriteRepositories(string username, string personalToken)
        {
            try
            {
                HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {personalToken}");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "HelloBuild");

                string apiUrl = $"https://api.github.com/users/{username}/starred";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<GitFavRepositoryResponse>? favoriteRepositories = JsonConvert.DeserializeObject<List<GitFavRepositoryResponse>>(content);

                    if (favoriteRepositories == null)
                    {
                        return (true, new List<GitRepositoryResponse>());
                    }

                    List<GitRepositoryResponse> favRepositories = _mapper.Map<List<GitRepositoryResponse>>(favoriteRepositories);

                    return (true, favRepositories);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return (false, new List<GitRepositoryResponse>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<GitRepositoryResponse>());
            }
        }

    }
}
