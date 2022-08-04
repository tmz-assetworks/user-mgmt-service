using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Responses.Users;
using MediatR;
namespace UsersService.Application.Commands.Users
{
    public class DeleteUserCommand :IRequest<UserResponse>   
    {
        public long Id { get; set; }
        
        public bool IsActive { get; set; }
    }
}
