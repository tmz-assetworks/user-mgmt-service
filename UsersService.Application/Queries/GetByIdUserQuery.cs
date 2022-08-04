using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;

namespace UsersService.Application.Queries
{
    public class GetByIdUserQuery : IRequest<Users>
    {
        public long Id { get; set; }
        public GetByIdUserQuery(int id)
        {
            Id = id;
        }
    }
}
