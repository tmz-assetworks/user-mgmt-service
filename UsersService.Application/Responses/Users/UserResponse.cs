
namespace UsersService.Responses.Users
{
    public partial class UserResponse
    {

        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public long Id { get; set; }
        public string OTP { get; set; }
    }
    public partial class UserProfileResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}
