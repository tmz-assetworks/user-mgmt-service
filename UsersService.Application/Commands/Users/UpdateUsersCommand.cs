
using MediatR;
using UsersService.Responses.Users;

namespace UsersService.Application.Commands.Users
{
   
    public class UpdateUserCommand : IRequest<UserResponse>   
    {
        public long Id { get; set; }
        public string EmailId { get; set; }
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
