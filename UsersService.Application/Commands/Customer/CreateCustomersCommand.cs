using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Responses.Customer;

namespace UsersService.Application.Commands.Customer
{
    public class CreateCustomersCommand : IRequest<CustomerResponse>
    {
        public string userName { get; set; }

        public DateTime DOB { get; set; }

        public long PhoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public long CountryID { get; set; }


        public long StateID { get; set; }


        public string City { get; set; }

        public string ZipCode { get; set; }

        public string CreatedBy { get; set; }


        public DateTime CreatedOn { get; set; }


        public string ModifiedBy { get; set; }


        public DateTime ModifiedOn { get; set; }
    }
}
