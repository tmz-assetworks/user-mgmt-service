using MediatR;
using UsersService.Core.Entities;

namespace UsersService.Application.Queries
{   
    public class AuthorizationUserQuery : IRequest<List<Users>>
    {

    }
}
