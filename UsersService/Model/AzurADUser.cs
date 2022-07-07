namespace VerifyAssetWorksAzureAD.Model
{
    public class AzurADUser
    {
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public string userPrincipalName { get; set; }
        public bool accountEnabled { get; set; }
        public string password { get; set; }
        public bool isForceChangePasswordNextSignIn { get; set; }
    }
}
