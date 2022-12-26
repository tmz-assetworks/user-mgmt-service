using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class UserProfileHandler : IRequestHandler<GetByUserProfileIdQuery, GetUserProfileResponseDT>
    {
        private readonly IUserRepository _userRepo;
        public UserProfileHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<GetUserProfileResponseDT> Handle(GetByUserProfileIdQuery request, CancellationToken cancellationToken)
        {
            return (GetUserProfileResponseDT)await _userRepo.GetByProfileUser();
        }
    }
}
