namespace AssetsService.UnitTests
{
    public static class TestModel
    {
        public static Dictionary<string, string> GetKey()
        {
            Dictionary<string,string> myConfiguration = new Dictionary<string, string>
                {
                    {"AzureAd:clientId", "7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed"},
                    {"AzureAd:TenantId", "744aa8b0-bb99-4982-903f-52328216b4be"},
                    {"AzureAd:clientSecret", "xg.8Q~mpJZWo5su4_WIKRaVaydfHP99xFa15uak_"},
                    {"EncryptDecryptkey", "E534C8DF286CD5931069B522E695D4F1"},
                };

            return myConfiguration;
        }
        public static string GetValue(object result)
        {
            var nameOfProperty = "Value";
            var propertyInfo = result.GetType().GetProperty(nameOfProperty);
            string value = propertyInfo.GetValue(result, null).ToString();


            return value;
        }

    }
   
   
}