
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UsersService.Core.Entities;
using UsersService.Core.Repositories;
using UsersService.Core.Repositories.Base;
using UsersService.Infrastructure.Repositories.Repository;

namespace UsersService.Infrastructure.Repositories.Customer
{
    public class CustomerRepository : Repository<Customers>, ICustomerRepository
    {

        public CustomerRepository(UsersService.Infrastructure.DBContext.DBContextCore dbContext) : base(dbContext)
        {

        }
       
        public async Task<Customers> GetByIdCustomers(long Custid)
        {

            return _dbContext.Customers
                 .Select(m => new Customers
                 {
                     Id = m.Id,
                     userName = m.userName,
                     DOB = m.DOB,
                     phoneNumber = m.phoneNumber,
                     AddressLine1 = m.AddressLine1,
                     AddressLine2 = m.AddressLine2,
                     CountryID = m.CountryID,
                     StateID = m.StateID,
                     city = m.city,
                     createdBy = m.createdBy,
                     createdOn = m.createdOn,
                     modifiedBy = m.modifiedBy,
                     modifiedOn = m.modifiedOn,
                     isActive = m.isActive,

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
                 }).Where(x => x.Id == Custid).FirstOrDefault();
        
        }
        public async Task<List<Customers>> GetAllCustomers()
        {
           return await _dbContext.Customers
                .Select(m => new Customers
                {
                    Id = m.Id,
                    userName = m.userName,
                    DOB = m.DOB,
                    phoneNumber = m.phoneNumber,
                    AddressLine1 = m.AddressLine1,
                    AddressLine2 = m.AddressLine2,
                    CountryID = m.CountryID,
                    StateID = m.StateID,
                    city = m.city,
                    createdBy = m.createdBy,
                    createdOn = m.createdOn,
                    modifiedBy = m.modifiedBy,
                    modifiedOn = m.modifiedOn,
                    isActive = m.isActive,

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
      
    }
}

