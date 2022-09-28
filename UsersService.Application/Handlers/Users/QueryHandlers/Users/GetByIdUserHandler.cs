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
using UsersService.Core.Response;

namespace UsersService.Application.Handlers.Users.QueryHandlers.Users
{
    public class GetByIdUserHandler : IRequestHandler<GetByIdUserQuery, GetUserResponseDT>
    {
        private readonly IUserRepository _userRepo;
        public GetByIdUserHandler(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<GetUserResponseDT> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            return (GetUserResponseDT)await _userRepo.GetByIdUser(request.Id);
        }
    }
}
