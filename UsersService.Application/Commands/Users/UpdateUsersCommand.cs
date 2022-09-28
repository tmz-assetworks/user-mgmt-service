
using MediatR;
using UsersService.Responses.Users;

namespace UsersService.Application.Commands.Users
{
   
    public class UpdateUserCommand : IRequest<UserResponse>   
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string EmailId { get; set; }
        public DateTime DOB { get; set; }
        public long PhoneNumber { get; set; }
        public long customerID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public long CityID { get; set; }
        public string ZipCode { get; set; }
        public string ModifiedBy { get; set; }
        public List<upUserRole> UserRolesCommand { get; set; }
        public List<upOperatorUserMapper>? operatorUserMapperCommand { get; set; }
    }
    public class upUserRole
    {
        public long Id { get; set; }
        public int RoleID { get; set; }
    }
    public class upOperatorUserMapper
    {
        public int id { get; set; }
        public long locationId { get; set; }
    }
}
