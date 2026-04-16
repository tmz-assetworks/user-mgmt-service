using MediatR;
using UsersService.Application.Commands.Users;
using UsersService.Core.Repositories.Users;

namespace UsersService.Application.Handlers.Users.CommandHandlers.Users
{
    public class DeleteUserByIdCommandHandler: IRequestHandler<DeleteUserByIdCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserByIdCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteAdminOrUser(request.UserId);
        }
    }
}
