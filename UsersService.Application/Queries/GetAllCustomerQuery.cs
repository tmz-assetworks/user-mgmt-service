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
    public class GetAllCustomerQuery : IRequest<AllCustomersResponse>
    {
        public GetAllCustomerRequest GetAllCustomerRequest { get; set; }

        public GetAllCustomerQuery(GetAllCustomerRequest getAllCustomerRequest)
        {
            GetAllCustomerRequest = getAllCustomerRequest;
        }   
    }
}
