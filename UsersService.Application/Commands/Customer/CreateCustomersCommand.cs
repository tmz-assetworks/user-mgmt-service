using MediatR;
using UsersService.Application.Responses.Customer;

namespace UsersService.Application.Commands.Customer
{
    public class CreateCustomersCommand : IRequest<CustomerResponse>
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string notes { get; set; }
        public string pointofcontact { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public string ZipCode { get; set; }
        public string CityName { get; set; }
        public string CreatedBy { get; set; }
        public int? TimeZoneID { get; set; }
    }
}
