using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;

namespace UsersService.Application.Responses.Customer
{
    public partial class CustomerResponse
    {

        public long Id { get; set; }
        public string UserName { get; set; }

        public DateTime DOB { get; set; }

        public long PhoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public long countryID { get; set; }
        public Countries Country { get; set; }

        public long StateID { get; set; }

        public States State { get; set; }
        public string city { get; set; }

        public string zipCode { get; set; }

        public string createdBy { get; set; }


        public DateTime CreatedOn { get; set; }


        public string modifiedBy { get; set; }


        public DateTime modifiedOn { get; set; }


        public bool IsActive { get; set; }
    }
}
