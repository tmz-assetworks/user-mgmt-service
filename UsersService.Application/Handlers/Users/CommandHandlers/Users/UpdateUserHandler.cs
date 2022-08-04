using UsersService.Core.Mapper;
using MediatR;
using UsersService.Application.Commands.Users;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Users;
using UsersService.Responses.Users;

namespace UsersService.Application.Handlers.Users.CommandHandlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponse>
    {
        private readonly IUserRepository _UserRepo;

        public UpdateUserHandler(IUserRepository UserRepository)
        {
            _UserRepo = UserRepository;
        }


        public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var UserEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            if (UserEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }

            var updateCable = _UserRepo.UpdateAsync(UserEntitiy, UserEntitiy.Id);
            var mapUserResponse = UsersMapper.Mapper.Map<UserResponse>(updateCable.Result);
            return mapUserResponse;
        }
        
    }
}
