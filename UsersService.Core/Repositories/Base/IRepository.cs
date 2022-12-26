
namespace UsersService.Core.Repositories.Base
{   
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity, long id);
        Task DeleteAsync(T entity);
        Task<T> DeleteUserAsync(T entity, long id,string types);
    }
}
