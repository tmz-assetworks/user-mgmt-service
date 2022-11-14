using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Queries;
using UsersService.Core.Mapper;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;
using UsersService.Responses.Users;

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
            //otpdata userResponse = new otpdata();
            //var UserEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            //if (UserEntitiy is null)
            //{
            //    throw new ApplicationException("Issue with mapper");
            //}
            //var updateUser = await _userRepo.Updateotp(UserEntitiy);
            var result = await _userRepo.Updateotp(request.Emailid, request.Otp);
            return result;
        }
    }
}
