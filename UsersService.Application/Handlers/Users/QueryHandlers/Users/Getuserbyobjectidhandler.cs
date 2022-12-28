using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class Getuserbyobjectidhandler : IRequestHandler<GetbyUserobjectID, GetUserobjectbyidDT>
    {
        private readonly IUserRepository _userRepo;
        public Getuserbyobjectidhandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<GetUserobjectbyidDT> Handle(GetbyUserobjectID request, CancellationToken cancellationToken)
        {
            return (GetUserobjectbyidDT)await _userRepo.GetUserbyobjectId(request.ObjectId);
        }
    }
}
