using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
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
