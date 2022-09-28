using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Responses.Customer;

namespace UsersService.Application.Commands.AuthPassword
{
    public class ResetPassword
    {
        public string emailid { get; set; }
        public string password { get; set; }
    }
}
