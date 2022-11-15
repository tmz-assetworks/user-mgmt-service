
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UsersService.Core.Entities;
using UsersService.Core.PagingHelper;
using UsersService.Core.Repositories;
using UsersService.Core.Response;
using UsersService.Infrastructure.Helpers;
using UsersService.Infrastructure.Repositories.Repository;

namespace UsersService.Infrastructure.Repositories.Customer
{
    public class CustomerRepository : Repository<Customers>, ICustomerRepository
    {
        TokenBase _tokenbase;
        public CustomerRepository(UsersService.Infrastructure.DBContext.DBContextCore dbContext, TokenBase tokenBase) : base(dbContext)
        {
            _tokenbase = tokenBase;
        }

        public async Task<AllCustomerResp> GetByIdCustomers(string Custid)
        {
            AllCustomerResp allCustomerResp = new AllCustomerResp();
            if (Custid != null && Custid != "")
            {
               var result = (from m in _dbContext.Customers
                              join country in _dbContext.Country
                              on m.CountryID equals country.Id
                              join state in _dbContext.State
                              on m.StateID equals state.Id
                              //join City in _dbContext.City
                              //on m.CityID equals City.Id
                              where (m.Id== Convert.ToInt64(Custid))
                              select new customerbyID
                              {
                                  Id = m.Id,
                                  UserName = m.userName,
                                  email = m.email,
                                  pointofcontact = m.pointofcontact,
                                  description = m.description,
                                  PhoneNumber = m.phoneNumber,
                                  AddressLine1 = m.AddressLine1,
                                  AddressLine2 = m.AddressLine2,
                                  zipCode = m.zipCode,
                                  isActive = m.isActive,
                                  CountryID = country.Id,
                                  countryName = country.countryName,
                                  StateID = state.Id,
                                  stateName = state.stateName,
                                  cityName = m.CityName == null ? "" : m.CityName,
                              }).FirstOrDefault();
                if (result != null)
                {
                    allCustomerResp.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerResp.StatusMessage = "Record Found";
                    allCustomerResp.data = new List<customerbyID>()
                {
                result,
                 };
                }
                else
                {
                    allCustomerResp.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerResp.StatusMessage = "Record not Found";
                    allCustomerResp.data = null;
                }
            }
            else
            {
               var result = (from m in _dbContext.Customers
                              join country in _dbContext.Country
                              on m.CountryID equals country.Id
                              join state in _dbContext.State
                              on m.StateID equals state.Id
                              join u in _dbContext.Users
                              on m.Id equals u.CustomerID
                              where (u.ObjectId == _tokenbase.getcustomerId())
                              select new customerbyID
                              {
                                  Id = m.Id,
                                  UserName = m.userName,
                                  email = m.email,
                                  pointofcontact = m.pointofcontact,
                                  description = m.description,
                                  PhoneNumber = m.phoneNumber,
                                  AddressLine1 = m.AddressLine1,
                                  AddressLine2 = m.AddressLine2,
                                  zipCode = m.zipCode,
                                  isActive = m.isActive,
                                  CountryID = country.Id,
                                  countryName = country.countryName,
                                  StateID = state.Id,
                                  stateName = state.stateName,
                                  cityName = m.CityName == null ? "" : m.CityName,
                              }).FirstOrDefault();
                if (result != null)
                {
                    allCustomerResp.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerResp.StatusMessage = "Record Found";
                    allCustomerResp.data = new List<customerbyID>()
                {
                result,
                 };
                }
                else
                {
                    allCustomerResp.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerResp.StatusMessage = "Record not Found";
                    allCustomerResp.data = null;
                }
            }

            
            return allCustomerResp;
        }
        public async Task<AllCustomersResponse> GetAllCustomers(GetAllCustomerRequest getAllCustomerRequest)
        {
            AllCustomersResponse allCustomerResp = new AllCustomersResponse();
            int padsCounts = _dbContext.Pads.Count();
            int powerCapinateCounts = _dbContext.PowerCabinet.Count();
            int modemCounts = _dbContext.Modem.Count();
            int cableCounts = _dbContext.Cables.Count();
            int rfIdReaerCounts = _dbContext.RFIDReaders.Count();
            int switchGearCount = _dbContext.SwitchGears.Count();
            int nocharger = _dbContext.Charger.Count();
            var result = (from m in _dbContext.Customers
                          join country in _dbContext.Country
                          on m.CountryID equals country.Id
                          join state in _dbContext.State
                          on m.StateID equals state.Id
                          select new allcustomerbyID
                          {
                              Id = m.Id,
                              UserName = m.userName,
                              email = m.email,
                              description = m.description,
                              pointofcontact = m.pointofcontact,
                              PhoneNumber = m.phoneNumber,
                              AddressLine1 = m.AddressLine1,
                              AddressLine2 = m.AddressLine2,
                              CountryID = country.Id,
                              countryName = country.countryName,
                              StateID = state.Id,
                              cityName = m.CityName==null?"": m.CityName,
                              stateName = state.stateName,
                              zipCode = m.zipCode,
                              noofevcharger = nocharger,//_dbContext.Dispenser.Count(),
                              assets = padsCounts + modemCounts + cableCounts + powerCapinateCounts + rfIdReaerCounts+ switchGearCount,
                              users = _dbContext.Users.Count(),
                              isActive = m.isActive,
                              modifiedOn = m.modifiedOn,
                          }).ToList<allcustomerbyID>();
            result = result != null ? result.OrderByDescending(x => x.modifiedOn).ToList() : result;
            allCustomerResp.InActive = result.Where(m => m.isActive == false).Count();
            allCustomerResp.Active = result.Where(m => m.isActive == true).Count();
            if (!string.IsNullOrEmpty(getAllCustomerRequest.SearchParam))
                result = result.Where(d => d.UserName.ToLower().Contains(getAllCustomerRequest.SearchParam.ToLower())
             ).ToList<allcustomerbyID>();
            allCustomerResp.data = PagedList<allcustomerbyID>.ToPagedList(result,
               getAllCustomerRequest.PageNumber,
               getAllCustomerRequest.PageSize);
            return allCustomerResp;
        }

    }
}

