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
    public class CreateCustomerHandler : IRequestHandler<CreateCustomersCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _custRepo;

        public CreateCustomerHandler(ICustomerRepository custRepository)
        {
            _custRepo = custRepository;
        }
        public async Task<CustomerResponse> Handle(CreateCustomersCommand request, CancellationToken cancellationToken)
        {
            
            var custEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Customers>(request);

            if (custEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }
            custEntitiy.isActive = true;//set newly created customer as a active
            custEntitiy.createdOn = DateTime.Now;
            custEntitiy.modifiedOn = DateTime.Now;
            custEntitiy.modifiedBy = "";
            var addcustResponse = await _custRepo.AddAsync(custEntitiy);
            var mapcustResponse = UsersMapper.Mapper.Map<CustomerResponse>(addcustResponse);
            return mapcustResponse;
        }
    }
    
}
