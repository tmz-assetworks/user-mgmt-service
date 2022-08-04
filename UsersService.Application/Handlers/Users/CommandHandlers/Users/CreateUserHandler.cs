using MediatR;
using UsersService.Application.Commands.Users;
using UsersService.Core.Entities;
using UsersService.Core.Mapper;
using UsersService.Core.Repositories.Users;
using UsersService.Responses.Users;

namespace UsersService.Application.Handlers.Assets.CommandHandlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepo;

        public CreateUserHandler(IUserRepository cableRepository)
        {
            _userRepo = cableRepository;
        }
        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            if (userEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }
            userEntitiy.IsActive = true;//set newly created customer as a active
            var addUserResponse = await _userRepo.AddAsync(userEntitiy);
            var mapUserResponse = UsersMapper.Mapper.Map<UserResponse>(addUserResponse);
            return mapUserResponse;
        }
    }
}
