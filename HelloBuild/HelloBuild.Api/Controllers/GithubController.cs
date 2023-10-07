using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelloBuild.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GithubController : ControllerBase
    {
        private readonly IGithubService _gitService;

        public GithubController(IGithubService gitService)
        {
            _gitService = gitService;
        }

        /// <summary>
        /// Get repositories.
        /// </summary>
        /// <param name="isbn">Book ID.</param>
        /// <param name="identificaciónUsuario">User ID</param>
        /// <param name="tipoUsuario">User Type</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("GetRepositories")]
        [ProducesResponseType(typeof(List<GitRepositoryResponse>), 200)]
        [ProducesResponseType(typeof(BadResponse), 400)]
        public async Task<IActionResult> GetRepositories(GitRepositoryRequest request)
        {
            (bool status, List<GitRepositoryResponse> response) = await _gitService.GetRepository(request.Username, request.PersonalAccessToken);

            return !status
                ? BadRequest(new BadResponse
                {
                    Message = "An error occurred trying to read this user's repository"
                })
                : Ok(response);
        }

    }
}
