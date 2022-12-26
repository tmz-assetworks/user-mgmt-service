using MediatR;
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
