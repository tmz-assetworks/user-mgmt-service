using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class UpdateOtpQuery:IRequest<otpdata>
    {
        public string Emailid { get; set; }
        public string Otp { get; set; }
        public UpdateOtpQuery(string emailid, string otp)
        {
            Emailid = emailid;
            Otp = otp;
        }
    }
}
