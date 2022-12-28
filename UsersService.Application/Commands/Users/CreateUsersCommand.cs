using MediatR;
using UsersService.Responses.Users;

namespace UsersService.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<UserResponse>
    {
        public string DisplayName { get; set; }
        public string? objectid { get; set; }
        public string userPrincipalName { get; set; }
        public string MailNickname { get; set; }
        public bool isActive { get; set; }
        public string EmailId { get; set; }
        public string name { get; set; }
        public string PhoneNumber { get; set; }
        public long CustomerID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string CreatedBy { get; set; }
        public List<UserRole> UserRolesCommand { get; set; }
        public List<long>? operatorUserMapperCommand { get; set; }
    }

    public class UserRole
    { 
        public long Roleid { get; set; }
    }
    public class OperatorUserMapper
    {
        public long locationId { get; set; }
    }

}
