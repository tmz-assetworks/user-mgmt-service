namespace VerifyAssetWorksAzureAD.Model
{
    public class UserModel
    {
        public string DisplayName { get; set; }
        public string userPrincipalName { get; set; }
        public string MailNickname { get; set; }
        public bool isActive { get; set; }
        public string Password { get; set; }
        public bool isForceChangePasswordNextSignIn { get; set; }
    }
}
