using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;

namespace UsersService.Application.Queries
{
    public class GetByIdCustomersQuery : IRequest<Customers>
    {
        public long Id { get; set; }
        public GetByIdCustomersQuery(int id)
        {
            Id = id;
        }
    }
}
