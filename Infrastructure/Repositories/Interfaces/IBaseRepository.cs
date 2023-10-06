using System.Linq.Expressions;

namespace HelloBuild.Infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(Guid id);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        Task<int> SaveChangesAsync();

        int SaveChanges();
    }
}
