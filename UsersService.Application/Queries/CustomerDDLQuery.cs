using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class CustomerDDLQuery : IRequest<List<CustomerData>>
    {

    }
}
