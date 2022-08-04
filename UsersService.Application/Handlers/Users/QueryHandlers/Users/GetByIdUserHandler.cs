using UsersService.Core.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Core.Repositories.Users;
using UsersService.Application.Queries;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class GetByIdUserHandler : IRequestHandler<GetByIdUserQuery, UsersService.Core.Entities.Users>
    {
        private readonly IUserRepository _userRepo;
        public GetByIdUserHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<UsersService.Core.Entities.Users> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            return (UsersService.Core.Entities.Users)await _userRepo.GetByIdUser(request.Id);
        }
    }
}
