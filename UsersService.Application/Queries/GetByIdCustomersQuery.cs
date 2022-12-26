using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class GetByIdCustomersQuery : IRequest<AllCustomerResp>
    {
        public string Id { get; set; }
        public GetByIdCustomersQuery(string id)
        {
            Id = id; 
        }
    }
}
