using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UsersService.Core.Entities;
using UsersService.Core.PagingHelper;

namespace UsersService.Core.Response
{
    public class AllCustomerResp
    {
        public AllCustomerResp()
        {
            data = new List<customerbyID>();
        }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<customerbyID> data { get; set; }
    }
    public class AllCustomersResponse
    {
        public int? StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public int? Active { get; set; }
        public int? InActive { get; set; }
        public List<StatusData> statusData { get; set; }
        public PagedList<allcustomerbyID> data { get; set; }
        public PaginationResponse paginationResponse { get; set; }
    }
    public class customerbyID
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string email { get; set; }      
        public string PhoneNumber { get; set; }
        public string pointofcontact { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long? CountryID { get; set; }
        public long? StateID { get; set; }
        //public long? CityID { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string cityName { get; set; }
        public string description { get; set; }
        public string zipCode { get; set; }
        public bool isActive { get; set; }

    }
    public class allcustomerbyID
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string email { get; set; }
        public string pointofcontact { get; set; }       
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public long? CountryID { get; set; }
        public long? StateID { get; set; }
        //public long? CityID { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string cityName { get; set; }
        public string description { get; set; }
        public DateTime modifiedOn { get; set; }

        public string zipCode { get; set; }
        public long noofevcharger { get; set; }
        public long assets { get; set; }
        public long users { get; set; }
        public bool isActive { get; set; }

    }

    public class CountryResponse
    {
        public long Id { get; set; }
        public string name { get; set; }
    }

    public class StateResponse
    {
        public long id { get; set; }
        public string name { get; set; }
    }
    public class CityResponse
    {
        public long id { get; set; }
        public string name { get; set; }
    }
    public class GetAllCustomerRequest:QueryStringParameters
    {
        public string? opratorid { get; set; }
        public string? SearchParam { get; set; }
    }
    public class StatusList
    {
        public List<StatusData> StatusData { get; set; }

    }
    public class StatusData
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
    }
}
