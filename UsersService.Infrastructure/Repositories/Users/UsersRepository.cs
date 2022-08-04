
using Microsoft.EntityFrameworkCore;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Users;
using UsersService.Infrastructure.Repositories.Repository;

namespace UsersService.Infrastructure.Repositories.Assets
{
    public class UsersRepository : Repository<Users>, IUserRepository
    {
        public UsersRepository(UsersService.Infrastructure.DBContext.DBContextCore dbContext) : base(dbContext)
        {

        }


        public async Task<IEnumerable<Users>> GetUserByEmailIdPassword(string emaildId, string password)
        {
            return await _dbContext.Users
                .Where(m => m.EmailId == emaildId)
                .ToListAsync();

        }
        public async Task<Users> GetByIdUser(long userid)
        {

            return _dbContext.Users
                 .Select(m => new Users
                 {
                     Id = m.Id,
                     EmailId = m.EmailId,
                     DOB = m.DOB,
                     PhoneNumber = m.PhoneNumber,
                     AddressLine1 = m.AddressLine1,
                     AddressLine2 = m.AddressLine2,
                     CountryID = m.CountryID,
                     StateID = m.StateID,
                     City = m.City,
                     CreatedBy = m.CreatedBy,
                     CreatedOn = m.CreatedOn,
                     ModifiedBy = m.ModifiedBy,
                     ModifiedOn = m.ModifiedOn,
                     IsActive = m.IsActive,

                     Country = (from obls in _dbContext.Countries.Where(x => x.Id == m.CountryID)
                                select new Countries
                                {
                                    Id = obls.Id,
                                    name = obls.name,
                                    createdBy = obls.createdBy,
                                    createdOn = obls.createdOn,
                                    modifiedBy = obls.modifiedBy,
                                    modifiedOn = obls.modifiedOn,

                                }).FirstOrDefault(),
                     State = (from obls in _dbContext.States.Where(x => x.id == m.StateID)
                              select new States
                              {
                                  id = obls.id,
                                  name = obls.name,
                                  createdBy = obls.createdBy,
                                  createdOn = obls.createdOn,
                                  modifiedBy = obls.modifiedBy,
                                  modifiedOn = obls.modifiedOn,

                              }).FirstOrDefault(),
                 }).Where(x => x.Id == userid).FirstOrDefault();

        }
        public async Task<List<Users>> GetAllUsers()
        {
            return await _dbContext.Users
                 .Select(m => new Users
                 {
                     Id = m.Id,
                     EmailId = m.EmailId,
                     DOB = m.DOB,
                     PhoneNumber = m.PhoneNumber,
                     AddressLine1 = m.AddressLine1,
                     AddressLine2 = m.AddressLine2,
                     CountryID = m.CountryID,
                     StateID = m.StateID,
                     City = m.City,
                     CreatedBy = m.CreatedBy,
                     CreatedOn = m.CreatedOn,
                     ModifiedBy = m.ModifiedBy,
                     ModifiedOn = m.ModifiedOn,
                     IsActive = m.IsActive,

                     Country = (from obls in _dbContext.Countries.Where(x => x.Id == m.CountryID)
                                select new Countries
                                {
                                    Id = obls.Id,
                                    name = obls.name,
                                    createdBy = obls.createdBy,
                                    createdOn = obls.createdOn,
                                    modifiedBy = obls.modifiedBy,
                                    modifiedOn = obls.modifiedOn,

                                }).FirstOrDefault(),
                     State = (from obls in _dbContext.States.Where(x => x.id == m.StateID)
                              select new States
                              {
                                  id = obls.id,
                                  name = obls.name,
                                  createdBy = obls.createdBy,
                                  createdOn = obls.createdOn,
                                  modifiedBy = obls.modifiedBy,
                                  modifiedOn = obls.modifiedOn,

                              }).FirstOrDefault(),
                 })
                 .ToListAsync();
        }

        public Task<Users> GetByObjectIdUser(string objectid)
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserPrincipalNameUser(string userPrincipalName)
        {
            throw new NotImplementedException();
        }
    }
}
