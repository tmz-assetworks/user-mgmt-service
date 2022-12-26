using MediatR;
using UsersService.Application.Responses.Customer;

namespace UsersService.Application.Commands.Customer
{
    public class DeleteCustomersCommand : IRequest<CustomerResponse>
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
