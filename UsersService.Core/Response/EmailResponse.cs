using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Core.Response
{
    public class EmailResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string useremail { get; set; }
        public string objid { get; set; }
        public bool isActive { get; set; }

    }
}
