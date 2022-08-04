using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;

namespace UsersService.Core.Queries
{
    public class GetAllUserQuery : IRequest<List<Users>>
    {
       
    }
}
