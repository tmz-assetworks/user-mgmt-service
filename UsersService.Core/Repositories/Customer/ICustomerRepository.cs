using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Base;
using UsersService.Core.Response;

namespace UsersService.Core.Repositories
{
    public interface ICustomerRepository : IRepository<UsersService.Core.Entities.Customers>
    {
        Task<AllCustomersResponse> GetAllCustomers(GetAllCustomerRequest getAllCustomerRequest);
        Task<AllCustomerResp> GetByIdCustomers(long id);
    }

}
