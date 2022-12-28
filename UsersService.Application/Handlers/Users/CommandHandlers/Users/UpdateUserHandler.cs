using UsersService.Core.Mapper;
using MediatR;
using UsersService.Application.Commands.Users;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;
using OperatorUserMapper = UsersService.Core.Entities.OperatorUserMapper;

namespace UsersService.Application.Handlers.Users.CommandHandlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUserRepository _UserRepo;
        public UpdateUserHandler(IUserRepository UserRepository)
        {
            _UserRepo = UserRepository;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            UpdateUserResponse updateuserResponse =new UpdateUserResponse();
            var UserEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            if (UserEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }
            UserEntitiy.CreatedOn = DateTime.Now;
            UserEntitiy.ModifiedOn = DateTime.Now;
            UserEntitiy.CreatedBy = UserEntitiy.ModifiedBy;
            UserEntitiy.ObjectId = UserEntitiy.ModifiedBy;
            List<UserRoles> userRoles = new List<UserRoles>();
            for (int i = 0; i < request.UserRolesCommand.Count(); i++)
            {
                userRoles.Add(new UserRoles()
                {
                    createdBy = request.ModifiedBy,
                    createdOn = DateTime.Now,
                    modifiedOn = DateTime.Now,
                    modifiedBy = request.ModifiedBy,
                    UserID = request.Id,
                    RoleID = request.UserRolesCommand[i].RoleID,
                    id = request.UserRolesCommand[i].Id,
                });
            }
            List<OperatorUserMapper> OperatorUserMapper = new List<OperatorUserMapper>();
            if (request.operatorUserMapperCommand != null)
            {
                for (int i = 0; i < request.operatorUserMapperCommand.Count(); i++)
                {
                    OperatorUserMapper.Add(new OperatorUserMapper()
                    {
                        Id = 0,
                        CreatedBy = request.ModifiedBy,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = request.ModifiedBy,
                        UserId = request.Id,
                        IsActive = true,
                        LocationId = request.operatorUserMapperCommand[i],
                    });
                }
            }
            UserEntitiy.OperatorUserMapper = OperatorUserMapper;
            UserEntitiy.UserRoles = userRoles;
            updateuserResponse = await _UserRepo.UpdateUser(UserEntitiy);
            return updateuserResponse;
        }
        
    }
}
