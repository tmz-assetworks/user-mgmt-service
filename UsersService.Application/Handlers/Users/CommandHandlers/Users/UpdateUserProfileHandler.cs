using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Commands.Users;
using UsersService.Application.Queries;
using UsersService.Core.Entities;
using UsersService.Core.Mapper;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;
using UsersService.Responses.Users;

namespace UsersService.Application.Handlers.Users.CommandHandlers.Users
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileResponse>
    {
        private readonly IUserRepository _UserRepo;
        public UpdateUserProfileHandler(IUserRepository UserRepository)
        {
            _UserRepo = UserRepository;
        }
        public async Task<UserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            UserProfileResponse userResponse = new UserProfileResponse();
            var UserEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            if (UserEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }
            var updateUser = await _UserRepo.UpdateUserProfile(UserEntitiy);
            return userResponse;
        }

    }
}
