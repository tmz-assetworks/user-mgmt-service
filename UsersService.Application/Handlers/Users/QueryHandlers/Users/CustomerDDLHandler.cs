using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;
using UsersService.Infrastructure.Repositories.Assets;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class CustomerDDLHandler:IRequestHandler<CustomerDDLQuery, List<CustomerData>>
    {
        private readonly IUserRepository _userRepo;
        public CustomerDDLHandler(IUserRepository userRepository)
        {
            _userRepo= userRepository;
        }
        public async Task<List<CustomerData>> Handle(CustomerDDLQuery request, CancellationToken cancellationToken)
        {
            return (List<CustomerData>)await _userRepo.GetCustomerDDL();
        }
    }
}
