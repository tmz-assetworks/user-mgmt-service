namespace UsersService.Core.Response
{
    public class EmailResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string useremail { get; set; }
        public string name { get; set; }
        public string objid { get; set; }
        public bool isActive { get; set; }
    }
}
