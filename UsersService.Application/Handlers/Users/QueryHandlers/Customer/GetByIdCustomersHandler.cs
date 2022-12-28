using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Customer
{
    public class GetByIdCustomersHandler : IRequestHandler<GetByIdCustomersQuery, AllCustomerResp>
    {
        private readonly ICustomerRepository _CustomersRepo; 
        public GetByIdCustomersHandler(ICustomerRepository CustomersRepository)
        {
            _CustomersRepo = CustomersRepository;
        }
        public async Task<AllCustomerResp> Handle(GetByIdCustomersQuery request, CancellationToken cancellationToken)
        {
            var data=await _CustomersRepo.GetByIdCustomers(request.Id);
            return data;
        }
    }
}
