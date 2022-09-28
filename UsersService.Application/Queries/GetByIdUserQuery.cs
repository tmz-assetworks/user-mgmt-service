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
    public class GetByIdUserQuery : IRequest<GetUserResponseDT>
    {
        public long Id { get; set; }
        public GetByIdUserQuery(int id)
        {
            Id = id;
        }
    }
}
