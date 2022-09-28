using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class CustomerDDLQuery : IRequest<List<CustomerData>>
    {

    }
}
