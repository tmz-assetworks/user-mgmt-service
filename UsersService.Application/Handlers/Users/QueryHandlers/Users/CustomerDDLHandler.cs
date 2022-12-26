using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

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
