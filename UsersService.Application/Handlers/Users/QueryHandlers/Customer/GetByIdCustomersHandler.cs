using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Queries;
using UsersService.Core.Repositories;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Customer
{
    public class GetByIdCustomersHandler : IRequestHandler<GetByIdCustomersQuery, UsersService.Core.Entities.Customers>
    {
        private readonly ICustomerRepository _CustomersRepo;
        public GetByIdCustomersHandler(ICustomerRepository CustomersRepository)
        {
            _CustomersRepo = CustomersRepository;
        }
        public async Task<UsersService.Core.Entities.Customers> Handle(GetByIdCustomersQuery request, CancellationToken cancellationToken)
        {
            return (UsersService.Core.Entities.Customers)await _CustomersRepo.GetByIdCustomers(request.Id);
        }
    }
}
