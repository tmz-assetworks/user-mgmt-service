using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Base;

namespace UsersService.Core.Repositories
{
    public interface ICustomerRepository : IRepository<UsersService.Core.Entities.Customers>
    {
        Task<List<Customers>> GetAllCustomers();
        Task<Customers> GetByIdCustomers(long id);
    }

}
