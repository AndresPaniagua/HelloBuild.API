using HelloBuild.Domain.Models;

namespace HelloBuild.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response> AddUser(UserRequest request);

        Task<Response> UserExist(UserExistRequest request);

    }
}
