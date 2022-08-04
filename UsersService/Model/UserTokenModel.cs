namespace UsersService.Api.Model
{
    public class UserTokenModel
    {
        public string objectid { get; set; }
        public string username { get; set; }
        public string emailid { get; set; }
        public List<string> roles { get; set; }


    }
}
