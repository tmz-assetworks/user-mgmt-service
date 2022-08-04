using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersService.Responses.Auth
{

    public class LoginRequest
    {
        public string userName { get; set; }
        public string userPassword { get; set; }
    }

    public class LoginResponse
    {
        public string userName { get; set; }
        public string userPassword { get; set; }
    }

}
