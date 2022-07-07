using Microsoft.Graph;

namespace UsersService.Api.Model
{
    public class UserModelAD
    {
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public string userPrincipalName { get; set; }
        public bool accountEnabled { get; set; }
        public PasswordProfile passwordProfile { get; set; }
    }
}
