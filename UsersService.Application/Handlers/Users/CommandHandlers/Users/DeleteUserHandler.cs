using UsersService.Core.Mapper;
using MediatR;
using UsersService.Application.Commands.Users;
using UsersService.Core.Repositories.Users;
using UsersService.Responses.Users;

namespace UsersService.Application.Handlers.Users.CommandHandlers.Users
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, UserResponse>
    {    
        private readonly IUserRepository _UserRepo;
        public DeleteUserHandler(IUserRepository UserRepository)
        {
            _UserRepo = UserRepository;
        }
        public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var UserEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            if (UserEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }
            var updateuser = _UserRepo.DeleteUserAsync(UserEntitiy, UserEntitiy.Id, "USER");
            var mapUserResponse = UsersMapper.Mapper.Map<UserResponse>(updateuser.Result);
            return mapUserResponse;
        }
    }
}
