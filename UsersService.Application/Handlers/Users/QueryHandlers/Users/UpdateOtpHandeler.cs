using MediatR;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class UpdateOtpHandeler : IRequestHandler<UpdateOtpQuery, otpdata>
    {
        private readonly IUserRepository _userRepo;
        public UpdateOtpHandeler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<otpdata> Handle(UpdateOtpQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepo.Updateotp(request.Emailid, request.Otp);
            return result;
        }
    }
}
