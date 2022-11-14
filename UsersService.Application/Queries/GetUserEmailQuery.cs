using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
