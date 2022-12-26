using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    internal class UserRoleDDL
    {
    }
    public class UserRoleDDLHandler : IRequestHandler<userroleDDLQuery, List<userrolesDDL>>
    {
        private readonly IUserRepository _userRepo;
        public UserRoleDDLHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<List<userrolesDDL>> Handle(userroleDDLQuery request, CancellationToken cancellationToken)
        {
            return (List<userrolesDDL>)await _userRepo.GetUserDDL();
        }
    }
}
