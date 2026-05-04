using Microsoft.EntityFrameworkCore;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Base;

namespace UsersService.Infrastructure.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UsersService.Infrastructure.DBContext.DBContextCore _dbContext;

        public Repository(UsersService.Infrastructure.DBContext.DBContextCore dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<T> UpdateAsync(T entity, long id)
        {
            var entry = _dbContext.Set<T>().Find(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteUserAsync(T entity, long id, string types)
        {
            if (types == "CUSTOMER")
            {
                var existing = await _dbContext.Set<Customers>().FindAsync(id);
                if (existing == null) return entity;

                _dbContext.Set<Customers>().Remove(existing);
            }
            else if (types == "USER")
            {
                var existing = await _dbContext.Set<Users>().FindAsync(id);
                if (existing == null) return entity;

                _dbContext.Set<Users>().Remove(existing);
            }

            await _dbContext.SaveChangesAsync();
            return entity; // keeping as per your requirement
        }
    }
}
