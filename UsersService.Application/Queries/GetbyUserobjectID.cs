using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class GetbyUserobjectID : IRequest<GetUserobjectbyidDT>
    {
        public string ObjectId { get; set; }
        public GetbyUserobjectID(string objectid)
        {
            ObjectId = objectid;
        }
    }
}
