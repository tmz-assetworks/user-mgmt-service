using MediatR;
using System;
using UsersService.Responses.Users;

namespace UsersService.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<UserResponse>
    {

        public string EmailId { get; set; }

        public DateTime DOB { get; set; }
        public string UserPrincipalName { get; set; }

        public string ObjectId { get; set; }
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
