using MediatR;

namespace UsersService.Application.Commands.Users
{
    public class DeleteUserByIdCommand: IRequest<bool>
    {
        public int UserId { get; set; }
    }
}
