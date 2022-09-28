using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Core.Response;

namespace UsersService.Core.Queries
{
    public class GetAllUserQuery : IRequest<AllUserResponse>
    {
       public GetUserRequest GetUserRequest { get; set; }
        public GetAllUserQuery (GetUserRequest getUserRequest)
        {
            GetUserRequest = getUserRequest;
        }
    }
}
