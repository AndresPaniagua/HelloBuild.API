using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Context;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelloBuild.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PersistenceContext context)
            : base(context) { }

        public async Task<bool> FinduserEmailPasswordAsync(UserExistRequest user)
        {
            return (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Password)) 
                && await _context.Users.AnyAsync((entity) => (entity.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase) && entity.Password.Equals(user.Password)));
        }
    }
}
