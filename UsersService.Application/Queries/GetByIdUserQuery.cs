using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class GetByIdUserQuery : IRequest<GetUserResponseDT>
    {
        public long Id { get; set; }
        public GetByIdUserQuery(int id)
        {
            Id = id;
        }
    }
}
