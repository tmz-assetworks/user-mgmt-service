using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Commands.Customer;
using UsersService.Application.Responses.Customer;
using UsersService.Core.Mapper;
using UsersService.Core.Repositories;

namespace UsersService.Application.Handlers.Users.CommandHandlers.Customer
{
    public class DeleteCustomersHandler : IRequestHandler<DeleteCustomersCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _CustomerRepo;

        public DeleteCustomersHandler(ICustomerRepository CustomerRepository)
        {
            _CustomerRepo = CustomerRepository;
        }
        public async Task<CustomerResponse> Handle(DeleteCustomersCommand request, CancellationToken cancellationToken)
        {
            var CustomersEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Customers>(request);
            if (CustomersEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }

            var updatecustomer = _CustomerRepo.DeleteUserAsync(CustomersEntitiy, CustomersEntitiy.Id,"CUSTOMER");
            var mapcustomerResponse = UsersMapper.Mapper.Map<CustomerResponse>(updatecustomer.Result);
            return mapcustomerResponse;
        }
    }
}
