using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Entities;
using UsersService.Core.PagingHelper;

namespace UsersService.Core.Response
{
    public class GetUserResponseDT
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public GetUserResponse data { get; set; }
    }
    public class GetUserobjectbyidDT
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public GetUserbyobjectidResponse data { get; set; }
    }
    public partial class GetUserbyobjectidResponse
    {
        public long Id { get; set; }
        public long cID { get; set; }
        public string Rolename { get; set; }
        public string ObjectID { get; set; }
    }
    public partial class GetUserResponse
    {
        public long Id { get; set; }
        public string EmailId { get; set; }
        //public string UserPrincipalName { get; set; }
        public string adminName { get; set; }

        public string PhoneNumber { get; set; }

        public string addressLine1 { get; set; }

        public string addressLine2 { get; set; }

        public long? CountryID { get; set; }
        public long? StateID { get; set; }
        //public long? CityID { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string cityName { get; set; }

        public string createdBy { get; set; }
        public string zipcode { get; set; }
        public DateTime CreatedOn { get; set; }


        public string modifiedBy { get; set; }


        public DateTime modifiedOn { get; set; }
        public long? customerID { get; set; }
        public string customername { get; set; }
        public bool IsActive { get; set; }
        public List<long> locationsId { get; set; }
    }

    public class CustomerDDL
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<CustomerData> data { get; set; }
    }

    public class CustomerData
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
    }
    public class AllUserResponse
    {
        public int? StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public int? Active { get; set; }
        public int? InActive { get; set; }
        public List<StatusData> statusData { get; set; }
        public PagedList<GetUserResponse> data { get; set; }
        public PaginationResponse paginationResponse { get; set; }
    }
    public class GetUserRequest : QueryStringParameters
    {
        public string? opratorid { get; set; }
        public int? customerID { get; set; }
        public List<long>? roleid { get; set; }
    }
   
    public class userrolesresponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<userrolesDDL> data { get; set; }
    }

    public class userrolesDDL
    {
        public long Id { get; set; }
        public long RoleID { get; set; }
    }
    public class otpdata
    {
        public string email { get; set; }
        public string obectId { get; set; }
        public string otp { get; set; }
    }

    public class GetUserProfileResponseDT
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public GetUserProfileResponse data { get; set; }
    }
    public partial class GetUserProfileResponse
    {
        public long Id { get; set; }
        public string EmailId { get; set; }
        public string adminName { get; set; }
        public string PhoneNumber { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public long? CountryID { get; set; }
        public long? StateID { get; set; }
        //public long? CityID { get; set; }
       
        public string countryName { get; set; }
        public string ImagePath { get; set; }
        public string stateName { get; set; }
        public string cityName { get; set; }
        public string zipcode { get; set; }
    }
    public partial class UpdateUserProfilePicture 
    {

        public string ProfilePicture { get; set; }
    }
}
