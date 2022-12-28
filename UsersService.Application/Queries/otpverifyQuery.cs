using MediatR;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class otpverifyQuery : IRequest<otpdata>
    {
        public string Emailid { get; set; }
        public string Otp { get; set; }
        public otpverifyQuery(string emailid, string otp)
        {
            Emailid = emailid;
            Otp = otp;
        }
    }
}
