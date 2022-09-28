
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UsersService.Core.Entities;
using UsersService.Core.PagingHelper;
using UsersService.Core.Repositories.Users;
using UsersService.Core.Response;
using UsersService.Infrastructure.Helpers;
using UsersService.Infrastructure.Repositories.Repository;

namespace UsersService.Infrastructure.Repositories.Assets
{
    public class UsersRepository : Repository<Users>, IUserRepository
    {
        TokenBase _tokenbase;
        public UsersRepository(UsersService.Infrastructure.DBContext.DBContextCore dbContext, TokenBase tokenBase) : base(dbContext)
        {
            _tokenbase = tokenBase;
        }


        public async Task<IEnumerable<Users>> GetUserByEmailIdPassword(string emaildId, string password)
        {
            return await _dbContext.Users
                .Where(m => m.EmailId == emaildId)
                .ToListAsync();

        }
        public async Task<GetUserResponseDT> GetByIdUser(long userid)
        {
            GetUserResponseDT getUserResponseDT = new GetUserResponseDT();
            var result = (from m in _dbContext.Users
                          join country in _dbContext.Country
                          on m.CountryID equals country.Id
                          join state in _dbContext.State
                          on m.StateID equals state.Id
                          join City in _dbContext.City
                          on m.CityID equals City.Id
                          join c in _dbContext.Customers
                          on m.CustomerID equals c.Id
                          select new GetUserResponse
                          {
                              Id = m.Id,
                              EmailId = m.EmailId,
                              adminName=m.name,
                              PhoneNumber = m.PhoneNumber,
                              addressLine1 = m.AddressLine1,
                              addressLine2 = m.AddressLine2,
                              createdBy = m.CreatedBy,
                              CreatedOn = m.CreatedOn,
                              modifiedBy = m.ModifiedBy,
                              modifiedOn = m.ModifiedOn,
                              IsActive = m.IsActive,
                              zipcode=m.ZipCode,
                              dob=m.DOB,
                              customerID=m.CustomerID,
                              customername = m.name,

                              CountryID = country.Id,
                              countryName = country.countryName,
                              StateID = state.Id,
                              CityID = City.Id,
                              cityName = City.cityName,
                              stateName = state.stateName,
                          }).Where(x => x.Id == userid).FirstOrDefault();

            if (result != null)
            {
                getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                getUserResponseDT.StatusMessage = "Record Found";
                getUserResponseDT.data = result;
            }
            else
            {
                getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                getUserResponseDT.StatusMessage = "Record not Found";
                getUserResponseDT.data = null;
            }
            return getUserResponseDT;
        }
        public async Task<AllUserResponse> GetAllUsers(GetUserRequest getUserRequest)
        {
            AllUserResponse allUserResponse = new AllUserResponse();
            List<GetUserResponse> result=new List< GetUserResponse >();
            if (_tokenbase.getrole().ToLower() == "admin")
            {
                 result = (from m in _dbContext.Users
                              join country in _dbContext.Country
                              on m.CountryID equals country.Id
                              join state in _dbContext.State
                              on m.StateID equals state.Id
                              join City in _dbContext.City
                              on m.CityID equals City.Id
                              join c in _dbContext.Customers
                              on m.CustomerID equals c.Id
                              join u in _dbContext.UserRoles
                              on m.Id equals u.UserID
                              where (u.RoleID == getUserRequest.roleid[0] && u.createdBy== _tokenbase.getobjectid())
                              select new GetUserResponse
                              {
                                  Id = m.Id,
                                  EmailId = m.EmailId,
                                  PhoneNumber = m.PhoneNumber,
                                  addressLine1 = m.AddressLine1,
                                  addressLine2 = m.AddressLine2,
                                  IsActive = m.IsActive,
                                  adminName = m.name,
                                  modifiedOn = m.ModifiedOn,
                                  customerID = m.CustomerID,
                                  customername = c.userName,
                                  CountryID = country.Id,
                                  countryName = country.countryName,
                                  StateID = state.Id,
                                  CityID = City.Id,
                                  cityName = City.cityName,
                                  stateName = state.stateName,
                              })
                     .ToList();
            }
            else
            {
                 result = (from m in _dbContext.Users
                              join country in _dbContext.Country
                              on m.CountryID equals country.Id
                              join state in _dbContext.State
                              on m.StateID equals state.Id
                              join City in _dbContext.City
                              on m.CityID equals City.Id
                              join c in _dbContext.Customers
                              on m.CustomerID equals c.Id
                              join u in _dbContext.UserRoles
                              on m.Id equals u.UserID
                              where (u.RoleID == getUserRequest.roleid[0])
                              select new GetUserResponse
                              {
                                  Id = m.Id,
                                  EmailId = m.EmailId,
                                  PhoneNumber = m.PhoneNumber,
                                  addressLine1 = m.AddressLine1,
                                  addressLine2 = m.AddressLine2,
                                  IsActive = m.IsActive,
                                  adminName = m.name,
                                  modifiedOn = m.ModifiedOn,
                                  customerID = m.CustomerID,
                                  customername = c.userName,
                                  CountryID = country.Id,
                                  countryName = country.countryName,
                                  StateID = state.Id,
                                  CityID = City.Id,
                                  cityName = City.cityName,
                                  stateName = state.stateName,
                              })
                                     .ToList();
            }
            result = result != null ? result.OrderByDescending(x => x.modifiedOn).ToList() : result;
            if (!string.IsNullOrEmpty(getUserRequest.SearchParam))
                result = result.Where(d => d.adminName.ToLower().Contains(getUserRequest.SearchParam.ToLower())
             ).ToList<GetUserResponse>();
            allUserResponse.data = PagedList<GetUserResponse>.ToPagedList(result,
               getUserRequest.PageNumber,
               getUserRequest.PageSize);
            allUserResponse.InActive = result.Where(m => m.IsActive == false).Count();
            allUserResponse.Active = result.Where(m => m.IsActive == true).Count();

            return allUserResponse;
        }

        public Task<Users> GetByObjectIdUser(string objectid)
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserPrincipalNameUser(string userPrincipalName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CustomerData>> GetCustomerDDL()
        {
            List<CustomerData> res = new List<CustomerData>();

            res = (from v in _dbContext.Customers
                   select new CustomerData
                   {
                       Id = v.Id,
                       CustomerName = v.userName,
                   }).OrderBy(a => a.CustomerName).ToList();

            return res;
        }

        public async Task<List<userrolesDDL>> GetUserDDL()
        {
            List<userrolesDDL> res = new List<userrolesDDL>();

            res = (from v in _dbContext.UserRoles
                   select new userrolesDDL
                   {
                       Id = v.id,
                       RoleID = v.RoleID,
                   }).ToList();

            return res;
        }

        public async Task<Users> UpdateUser(Users users)
        {
            try
            {

                Users ousers = _dbContext.Users.Find(users.Id);
                ousers.name= users.name;
                ousers.EmailId=users.EmailId;
                ousers.PhoneNumber=users.PhoneNumber;
                ousers.AddressLine1 = users.AddressLine1;
                ousers.AddressLine2 = users.AddressLine2;
                ousers.CityID=users.CityID;
                ousers.StateID=users.StateID;
                ousers.CountryID=users.CountryID;
                ousers.DOB=users.DOB;
                ousers.CustomerID=users.CustomerID;
                ousers.ZipCode=users.ZipCode;
                ousers.ModifiedBy=users.ModifiedBy;
                ousers.ModifiedOn = DateTime.Now;
                for (int i = 0; i < users.OperatorUserMapper.Count(); i++)
                {
                    UserRoles oldUserrole = _dbContext.UserRoles.Find(users.UserRoles.ToList()[i].id);
                    if (oldUserrole != null)
                    {
                        UserRoles newuser = users.UserRoles.ToList()[i];
                        oldUserrole.UserID = users.Id;
                        oldUserrole.modifiedBy = newuser.modifiedBy;
                        oldUserrole.modifiedOn = DateTime.Now;
                        oldUserrole.RoleID = newuser.RoleID;
                    }
                }
                for (int i = 0; i < users.OperatorUserMapper.Count(); i++)
                {
                    OperatorUserMapper oldOperatorUser = _dbContext.OperatorUserMapper.Find(users.OperatorUserMapper.ToList()[i].Id);
                    if (oldOperatorUser != null)
                    {
                        OperatorUserMapper newoperators = users.OperatorUserMapper.ToList()[i];
                        oldOperatorUser.UserId = users.Id;
                        oldOperatorUser.ModifiedBy = newoperators.ModifiedBy;
                        oldOperatorUser.LocationId = newoperators.LocationId;
                        oldOperatorUser.ModifiedBy = newoperators.ModifiedBy;
                        oldOperatorUser.ModifiedOn = DateTime.Now;
                    }
                }
                _dbContext.Update(ousers);
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            return (users);

        }

        public async Task<GetUserobjectbyidDT> GetUserbyobjectId(string userid)
        {
            GetUserobjectbyidDT getUserResponseDT = new GetUserobjectbyidDT();
            var result = (from m in _dbContext.Users
                          join u in _dbContext.UserRoles
                          on m.Id equals u.UserID
                          join r in _dbContext.Roles
                          on u.RoleID equals r.Id
                          select new GetUserbyobjectidResponse
                          {
                              Id = m.Id,
                              ObjectID = m.ObjectId,
                              Rolename=r.Name,
                              cID=(long)m.CustomerID,
                          }).Where(x => x.ObjectID == userid).FirstOrDefault();

            if (result != null)
            {
                getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                getUserResponseDT.StatusMessage = "Record Found";
                getUserResponseDT.data = result;
            }
            else
            {
                getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                getUserResponseDT.StatusMessage = "Record not Found";
                getUserResponseDT.data = null;
            }
            return getUserResponseDT;
        }
    }
}
