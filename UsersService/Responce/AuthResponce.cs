namespace UsersService.Api.Responce
{
    public class AuthResponce
    {
        public AuthResponce()
        {
            data = new List<AuthData>();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<AuthData> data { get; set; }
    }
    public class AuthData
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string resource { get; set; }
        public string id_token { get; set; }
    }
}
