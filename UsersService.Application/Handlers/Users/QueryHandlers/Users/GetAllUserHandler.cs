using UsersService.Core.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Users;

namespace UsersService.Application.Handlers.Assets.QueryHandlers
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, List<UsersService.Core.Entities.Users>>
    {
        private readonly IUserRepository _userRepo;

        public GetAllUserHandler(IUserRepository UserRepository)
        {
            _userRepo = UserRepository;
        }
        public async Task<List<UsersService.Core.Entities.Users>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return (List<UsersService.Core.Entities.Users>)await _userRepo.GetAllUsers();
        }
       
    }
}
