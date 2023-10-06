using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;

namespace HelloBuild.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> FinduserEmailPasswordAsync(UserExistRequest user);
    }
}
