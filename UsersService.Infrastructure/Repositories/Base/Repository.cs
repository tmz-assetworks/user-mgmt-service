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
        public async Task<T> DeleteUserAsync(T entity, long id,string types)
        {
            if (types == "CUSTOMER")
            {
                Customers old = entity as Customers;
                Customers u = _dbContext.Set<Customers>().Find(id);
                u.isActive = old.isActive;
                _dbContext.Entry(u);
                
            }
            else if(types == "USER")
            {
                Users old = entity as Users;
                Users u = _dbContext.Set<Users>().Find(id);
                u.IsActive = old.IsActive;
                _dbContext.Entry(u);
                
            }


            //var entry = _dbContext.Set<T>().Find(id);

            //_dbContext.Entry(entry).CurrentValues.SetValues(entity);
            //await _dbContext.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();
            return entity;


        }
    }
}
