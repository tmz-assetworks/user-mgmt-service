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
