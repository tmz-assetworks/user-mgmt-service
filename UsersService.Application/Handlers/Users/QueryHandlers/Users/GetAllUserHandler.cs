using UsersService.Core.Queries;
using MediatR;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Assets.QueryHandlers
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, AllUserResponse>
    {
        private readonly IUserRepository _userRepo;
        public GetAllUserHandler(IUserRepository UserRepository)
        {
            _userRepo = UserRepository;
        }
        public async Task<AllUserResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var result= (AllUserResponse)await _userRepo.GetAllUsers(request.GetUserRequest);
            return result;  
        }       
    }
}
