using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;

namespace UsersService.Application.Handlers.Assets.QueryHandlers
{
    public class AuthorizationUserHandler : IRequestHandler<AuthorizationUserQuery, List<UsersService.Core.Entities.Users>>
    {      
        private readonly IUserRepository _userRepo;
        public AuthorizationUserHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<List<Core.Entities.Users>> Handle(AuthorizationUserQuery request, CancellationToken cancellationToken)
        {
            return (List<Core.Entities.Users>)await _userRepo.GetAllAsync();
        }     
    }
}