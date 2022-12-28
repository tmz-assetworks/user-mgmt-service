using MediatR;
using Microsoft.AspNetCore.Http;
using UsersService.Core.Response;
using UsersService.Responses.Users;

namespace UsersService.Application.Commands.Users
{   
    public class UpdateUserCommand : IRequest<UpdateUserResponse>   
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public long customerID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string ModifiedBy { get; set; }
        public List<upUserRole> UserRolesCommand { get; set; }
        public List<long>? operatorUserMapperCommand { get; set; }
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
    public class UpdateUserProfileCommand : IRequest<UserProfileResponse>
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string ModifiedBy { get; set; }
        public bool NotificationEnable { get; set; }
    }
    public class UpdateUserProfileImage : IRequest<UserProfileResponse>
    {        
        public string ImagePath { get; set; }
    }
    public class UpdatedUserProfileImage : IRequest<UserProfileResponse>
    {
        public IFormFile ProfilePicture { get; set; }
    }
}
