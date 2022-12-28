using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class GetAllCustomerQuery : IRequest<AllCustomersResponse>
    {
        public GetAllCustomerRequest GetAllCustomerRequest { get; set; }

        public GetAllCustomerQuery(GetAllCustomerRequest getAllCustomerRequest)
        {
            GetAllCustomerRequest = getAllCustomerRequest;
        }   
    }
}
