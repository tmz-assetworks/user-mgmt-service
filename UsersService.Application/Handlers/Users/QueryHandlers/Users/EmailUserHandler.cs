using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class EmailUserHandler : IRequestHandler<GetUserEmailQuery, EmailResponse>
    {
        private readonly IUserRepository _userRepo;
        public EmailUserHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<EmailResponse> Handle(GetUserEmailQuery request, CancellationToken cancellationToken)
        {
            return (EmailResponse)await _userRepo.GetUserEmail(request.Useremail);
        }
    }
}
