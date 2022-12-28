using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class GetUserEmailQuery : IRequest<EmailResponse>
    {
        public string Useremail { get; set; }
        public GetUserEmailQuery(string useremail)
        {
            Useremail = useremail;
        }
    }
}
