using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Queries;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Users;
using UsersService.Responses.Users;

namespace UsersService.Application.Handlers.Assets.QueryHandlers
{
    public class AuthorizationUserHandler : IRequestHandler<AuthorizationUserQuery, List<UsersService.Core.Entities.Users>>
    {
      
            private readonly IUserRepository _userRepo;

        public AuthorizationUserHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<List<Core.Entities.Users>> Handle(AuthorizationUserQuery request, CancellationToken cancellationToken)
        {
            return (List<Core.Entities.Users>)await _userRepo.GetAllAsync();
        }

       
    }
}