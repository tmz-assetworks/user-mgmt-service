using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class GetByUserProfileIdQuery : IRequest<GetUserProfileResponseDT>
    {
        public GetByUserProfileIdQuery()
        {
        }
    }
}
