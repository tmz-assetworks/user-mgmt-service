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
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _CustRepo;

        public UpdateCustomerHandler(ICustomerRepository CustomerRepository)
        {
            _CustRepo = CustomerRepository;
        }


        public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var CustEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Customers>(request);
            if (CustEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }
            CustEntitiy.createdOn = DateTime.Now;
            CustEntitiy.modifiedOn = DateTime.Now;
            CustEntitiy.createdBy = CustEntitiy.modifiedBy;
            var updateCust = _CustRepo.UpdateAsync(CustEntitiy, CustEntitiy.Id);
            var mapUserResponse = UsersMapper.Mapper.Map<CustomerResponse>(updateCust.Result);
            return mapUserResponse;
        }
    }
}
