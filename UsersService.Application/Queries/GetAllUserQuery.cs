using MediatR;
using UsersService.Core.Response;

namespace UsersService.Core.Queries
{
    public class GetAllUserQuery : IRequest<AllUserResponse>
    {
       public GetUserRequest GetUserRequest { get; set; }
        public GetAllUserQuery (GetUserRequest getUserRequest)
        {
            GetUserRequest = getUserRequest;
        }
    }
}
