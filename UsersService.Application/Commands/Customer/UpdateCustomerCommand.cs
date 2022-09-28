using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Responses.Customer;

namespace UsersService.Application.Commands.Customer
{
    public class UpdateCustomerCommand : IRequest<CustomerResponse>
    {
        public long id { get; set; }
        public string userName { get; set; }
        public string pointofcontact { get; set; }
        public string email { get; set; }
        public string notes { get; set; }

        public long PhoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public long CountryID { get; set; }


        public long StateID { get; set; }


        public long CityID { get; set; }

        public string ZipCode { get; set; }

        public string ModifiedBy { get; set; }
    }
}
