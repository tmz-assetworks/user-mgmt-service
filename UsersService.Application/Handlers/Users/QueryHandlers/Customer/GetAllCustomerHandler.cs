using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Customer
{
    public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, AllCustomersResponse>
    {
        private readonly ICustomerRepository _CustomerRepo;

        public GetAllCustomerHandler(ICustomerRepository CustomerRepository)
        {
            _CustomerRepo = CustomerRepository;
        }
        public async Task<AllCustomersResponse> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            var result= (AllCustomersResponse)await _CustomerRepo.GetAllCustomers(request.GetAllCustomerRequest);
            return result;
        }
    }
}
