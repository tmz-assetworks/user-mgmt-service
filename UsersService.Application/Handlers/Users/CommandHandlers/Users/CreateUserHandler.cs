using MediatR;
using UsersService.Application.Commands.Users;
using UsersService.Core.Entities;
using UsersService.Core.Mapper;
using UsersService.Core.Repositories.Users;
using UsersService.Responses.Users;
using OperatorUserMapper = UsersService.Core.Entities.OperatorUserMapper;
using Extensions = UsersService.Infrastructure.Helpers.Extensions;

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
            var rotp = Extensions.Getrandomnumber();
            var userEntitiy = UsersMapper.Mapper.Map<UsersService.Core.Entities.Users>(request);
            if (userEntitiy is null)
            {
                throw new ApplicationException("Issue with mapper");
            }

            userEntitiy.IsActive = true;//set newly created customer as a active
            userEntitiy.CreatedOn = DateTime.Now;
            userEntitiy.ModifiedOn = DateTime.Now;
            userEntitiy.ModifiedBy = "";
            userEntitiy.ObjectId = request.objectid;
            userEntitiy.UserPrincipalName = request.userPrincipalName;
            userEntitiy.UserRoles = userEntitiy.UserRoles;
            userEntitiy.OtpDateTime = DateTime.Now;
            userEntitiy.Otp = rotp.ToString();
            List<UserRoles> userRoles = new List<UserRoles>();
            for (int i = 0; i < request.UserRolesCommand.Count(); i++)
            {
                userRoles.Add(new UserRoles()
                {
                    createdBy = request.CreatedBy,
                    createdOn = DateTime.Now,
                    modifiedOn = DateTime.Now,
                    modifiedBy = "",
                    UserID = 0,
                    RoleID = request.UserRolesCommand[i].Roleid,
                    id = 0,
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
                        CreatedBy = request.CreatedBy,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = "",
                        UserName = "NA",
                        UserId = 0,
                        IsActive = true,
                        LocationId = request.operatorUserMapperCommand[i],
                    });
                }
            }
            userEntitiy.OperatorUserMapper = OperatorUserMapper;
            userEntitiy.UserRoles = userRoles;
            var addUserResponse = await _userRepo.AddAsync(userEntitiy);
            var mapUserResponse = UsersMapper.Mapper.Map<UserResponse>(addUserResponse);
            return mapUserResponse;
        }
    }
}
