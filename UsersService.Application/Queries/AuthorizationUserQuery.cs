using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Responses.Auth;

namespace UsersService.Application.Queries
{
   
    public class AuthorizationUserQuery : IRequest<List<Users>>
    {

    }
}
