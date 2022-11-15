using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Queries;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class verifyotpHandler : IRequestHandler<otpverifyQuery, otpdata>
    {
        private readonly IUserRepository _userRepo;
        public verifyotpHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<otpdata> Handle(otpverifyQuery request, CancellationToken cancellationToken)
        {
            var result= (otpdata)await _userRepo.Getotp(request.Emailid,request.Otp);
            return result;
        }
    }
}
