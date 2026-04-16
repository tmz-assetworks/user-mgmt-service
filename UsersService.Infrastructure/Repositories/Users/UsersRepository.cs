using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
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
        public async Task<IEnumerable<Users>> GetUserByEmailIdPassword(string emailId, string password)
        {
            return await _dbContext.Users
                .Where(m => m.EmailId == emailId)
                .ToListAsync();
        }
        public async Task<GetUserResponseDT> GetByIdUser(long id)
        {
            GetUserResponseDT getUserResponseDT = new GetUserResponseDT();
            var result = (from m in _dbContext.Users
                          join country in _dbContext.Country
                          on m.CountryID equals country.Id into countryJoin
                          from country in countryJoin.DefaultIfEmpty()
                          join state in _dbContext.State
                          on m.StateID equals state.Id into stateJoin
                          from state in stateJoin.DefaultIfEmpty()
                          join c in _dbContext.Customers
                          on m.CustomerID equals c.Id
                          select new GetUserResponse
                          {
                              Id = m.Id,
                              EmailId = m.EmailId,
                              adminName = m.name,
                              PhoneNumber = m.PhoneNumber,
                              addressLine1 = m.AddressLine1,
                              addressLine2 = m.AddressLine2,
                              createdBy = m.CreatedBy,
                              CreatedOn = m.CreatedOn,
                              modifiedBy = m.ModifiedBy,
                              modifiedOn = m.ModifiedOn,
                              IsActive = m.IsActive,
                              zipcode = m.ZipCode,
                              customerID = m.CustomerID,
                              customername = m.name,
                              CountryID = m.CountryID,
                              countryName = country != null ? country.countryName : "",
                              StateID = m.StateID,
                              locationsId = m.OperatorUserMapper.ToList().Select(r => r.LocationId).ToList<long>(),
                              cityName = m.CityName == null ? "" : m.CityName,
                              stateName = state != null ? state.stateName : "",
                          }).Where(x => x.Id == id).FirstOrDefault();
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
            List<GetUserResponse> result = new List<GetUserResponse>();
            result = (from us in _dbContext.Users
                      join country in _dbContext.Country
                      on us.CountryID equals country.Id into countryjoin
                      from country in countryjoin.DefaultIfEmpty()
                      join state in _dbContext.State
                      on us.StateID equals state.Id into statejoin
                      from state in statejoin.DefaultIfEmpty()
                      join c in _dbContext.Customers
                      on us.CustomerID equals c.Id
                      join u in _dbContext.UserRoles
                      on us.Id equals u.UserID
                      where (u.RoleID == getUserRequest.roleid[0])
                      select new GetUserResponse
                      {
                          Id = us.Id,
                          EmailId = us.EmailId,
                          PhoneNumber = us.PhoneNumber,
                          addressLine1 = us.AddressLine1,
                          addressLine2 = us.AddressLine2,
                          IsActive = us.IsActive,
                          adminName = us.name,
                          modifiedOn = us.ModifiedOn,
                          customerID = us.CustomerID,
                          customername = c.userName,
                          CountryID = us.CountryID,
                          countryName = country != null ? country.countryName : "",
                          StateID = us.StateID,
                          cityName = us.CityName == null ? "" : us.CityName,
                          stateName = state != null ? state.stateName : "",
                          locationsName = String.Join(",", us.OperatorUserMapper.ToList().Join(_dbContext.Locations, a => a.LocationId, b => b.Id, (a, b) => b.LocationName)),
                          locationsNameCount = us.OperatorUserMapper.ToList().Join(_dbContext.Locations, a => a.LocationId, b => b.Id, (a, b) => b.LocationName).Count(),
                      })
                    .ToList();

            result = result != null ? result.OrderByDescending(x => x.modifiedOn).DistinctBy(p => new { p.Id }).ToList() : result.DistinctBy(p => new { p.Id }).ToList();
            allUserResponse.InActive = result.Where(m => m.IsActive == false).Count();
            allUserResponse.Active = result.Where(m => m.IsActive == true).Count();
            if (!string.IsNullOrEmpty(getUserRequest.SearchParam))
                result = result.Where(d => d.adminName.ToLower().Contains(getUserRequest.SearchParam.ToLower())
             ).ToList<GetUserResponse>();
            allUserResponse.data = PagedList<GetUserResponse>.ToPagedList(result,
               getUserRequest.PageNumber,
               getUserRequest.PageSize);

            return allUserResponse;
        }
        /// <summary>
        /// this is for getting userID
        /// </summary>
        /// <param name="objectid">Admin ID</param>
        /// <returns></returns>
        public async Task<Users> GetByObjectIdUser(string objectid)
        {

            return await _dbContext.Users.Where(x => x.ObjectId == objectid).FirstOrDefaultAsync();
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

        public async Task<UpdateUserResponse> UpdateUser(Users users)
        {
            UpdateUserResponse updateuserResponse = new UpdateUserResponse();
            try
            {
                string IsExist = "";
                Users ousers = _dbContext.Users.Find(users.Id);
                List<Users> list = _dbContext.Users.Where(u=>u.Id!=users.Id && u.EmailId==users.EmailId).ToList<Users>();
                if (list.Count>0)
                {
                        IsExist = " email already exist";
                }
                if (string.IsNullOrEmpty(IsExist))
                {
                    ousers.name = users.name;
                    ousers.EmailId = users.EmailId;
                    ousers.PhoneNumber = users.PhoneNumber;
                    ousers.AddressLine1 = users.AddressLine1;
                    ousers.AddressLine2 = users.AddressLine2;
                    ousers.CityName = users.CityName;
                    ousers.StateID = users.StateID;
                    ousers.CountryID = users.CountryID;
                    ousers.CustomerID = users.CustomerID;
                    ousers.ZipCode = users.ZipCode;
                    ousers.ModifiedBy = users.ModifiedBy;
                    ousers.ModifiedOn = DateTime.Now;
                    for (int i = 0; i < users.UserRoles.Count(); i++)
                    {
                        UserRoles oldUserrole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserID == users.UserRoles.ToList()[i].UserID && ur.RoleID == users.UserRoles.ToList()[i].RoleID);
                        if (oldUserrole != null)
                        {
                            UserRoles newuser = users.UserRoles.ToList()[i];
                            oldUserrole.UserID = users.Id;
                            oldUserrole.modifiedBy = newuser.modifiedBy;
                            oldUserrole.modifiedOn = DateTime.Now;
                            oldUserrole.RoleID = newuser.RoleID;
                        }
                    }

                    List<OperatorUserMapper> collec = _dbContext.OperatorUserMapper.Where(m => m.UserId == users.Id).ToList<OperatorUserMapper>();
                    foreach (OperatorUserMapper s in collec)
                    {
                        _dbContext.Set<OperatorUserMapper>().Remove(s);
                    }
                    for (int i = 0; i < users.OperatorUserMapper.Count(); i++)
                    {
                        OperatorUserMapper oldOperatorUser = new OperatorUserMapper();
                        OperatorUserMapper newoperators = users.OperatorUserMapper.ToList()[i];
                        oldOperatorUser.UserId = users.Id;
                        oldOperatorUser.ModifiedBy = newoperators.ModifiedBy;
                        oldOperatorUser.LocationId = newoperators.LocationId;
                        oldOperatorUser.ModifiedBy = newoperators.ModifiedBy;
                        oldOperatorUser.ModifiedOn = DateTime.Now;
                        oldOperatorUser.CreatedBy = newoperators.ModifiedBy;
                        oldOperatorUser.CreatedOn = DateTime.Now;
                        oldOperatorUser.IsActive = true;
                        _dbContext.Set<OperatorUserMapper>().Add(oldOperatorUser);
                }
                _dbContext.Update(ousers);
                _dbContext.SaveChanges();
                if(users.UserRoles.Count() > 0)
                {
                    updateuserResponse.StatusCode = 200;
                    updateuserResponse.StatusMessage = "Record updated successfully";
                    updateuserResponse.OTP = users.Otp;
                    updateuserResponse.Id = users.Id;
                }
                else
                {
                    updateuserResponse.StatusCode = 200;
                    updateuserResponse.StatusMessage = "Record not updated";
                    updateuserResponse.OTP = users.Otp;
                    updateuserResponse.Id = users.Id;
                }
                }
                else
                {
                    updateuserResponse.StatusCode = 400;
                    updateuserResponse.StatusMessage = "Email already exist";
                    updateuserResponse.OTP = users.Otp;
                    updateuserResponse.Id = users.Id;
                }
            }
            catch (Exception ex)
            {
                updateuserResponse.StatusCode = 400;
                updateuserResponse.StatusMessage = "Please enter valid mailid";
                updateuserResponse.OTP = users.Otp;
                updateuserResponse.Id = users.Id;
            }
            return updateuserResponse;
        }
        public async Task<otpdata> Getotp(string userid, string Otp)
        {
            otpdata getotpdata = new otpdata();
            var result = await (from m in _dbContext.Users
                                select new otpdata
                                {
                                    email = m.UserPrincipalName,
                                    obectId = m.ObjectId,
                                    otp = m.Otp,
                                }).Where(x => x.email.ToLower() == userid.ToLower() && x.otp == Otp).FirstOrDefaultAsync();
            getotpdata = result;
            return getotpdata;
        }

        public async Task<otpdata> Updateotp(string Emailid, string Otp)
        {
            Users Users = new Users();
            otpdata otpdata = new otpdata();
            try
            {
                Users = _dbContext.Users.FirstOrDefault(m => m.UserPrincipalName.ToLower() == Emailid.ToLower());
                Users.Otp = Otp;
                Users.OtpDateTime = DateTime.Now;
                _dbContext.Update(Users);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            otpdata.otp = Users.Otp;
            return otpdata;
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
                              Rolename = r.Name,
                              cID = (long)m.CustomerID,
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
        public async Task<EmailResponse> GetUserEmail(string Useremail)
        {
            EmailResponse getEmailResponse = new EmailResponse();
            var result = (from m in _dbContext.Users
                          where (m.EmailId == Useremail)
                          select new EmailResponse
                          {
                              useremail = m.UserPrincipalName,
                              isActive = m.IsActive,
                              objid = m.ObjectId,
                              name=m.name,
                          }).FirstOrDefault();
            if (result != null)
            {
                getEmailResponse.StatusCode = (int)HttpStatusCode.OK;
                getEmailResponse.StatusMessage = "Record Found";
                getEmailResponse.useremail = result.useremail;
                getEmailResponse.isActive = result.isActive;
                getEmailResponse.objid = result.objid;
                getEmailResponse.name = result.name;
            }
            else
            {
                getEmailResponse.StatusCode = (int)HttpStatusCode.OK;
                getEmailResponse.StatusMessage = "Record not Found";
                getEmailResponse.isActive = false;
                getEmailResponse.useremail = null;
                getEmailResponse.objid = null;
                getEmailResponse.name = null;
            }
            return getEmailResponse;
        }

        public async Task<long> GetUserRoleID(long UserID, long RoleID)
        {
            var result = await (from m in _dbContext.UserRoles
                                where m.UserID == UserID && m.RoleID == RoleID
                                select (long?)m.id).FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<GetUserProfileResponseDT> GetByProfileUser()
        {
            GetUserProfileResponseDT getUserResponseDT = new GetUserProfileResponseDT();
            var result = (from m in _dbContext.Users
                          join c in _dbContext.Country
                          on m.CountryID equals c.Id into countryjoin
                          from country in countryjoin.DefaultIfEmpty()
                          join s in _dbContext.State
                          on m.StateID equals s.Id into statejoin
                          from state in statejoin.DefaultIfEmpty()
                          join d in _dbContext.TimeZones
                          on m.Customer.TimeZoneID equals d.Id into detailsGroup
                          from detail in detailsGroup.DefaultIfEmpty()
                          where (m.ObjectId == _tokenbase.getobjectid())
                          select new GetUserProfileResponse
                          {
                              Id = m.Id,
                              EmailId = m.EmailId,
                              adminName = m.name,
                              PhoneNumber = m.PhoneNumber,
                              addressLine1 = m.AddressLine1,
                              addressLine2 = m.AddressLine2,
                              CountryID = country == null ? 0 : country.Id,
                              countryName = country == null ? "" : country.countryName,
                              StateID = state == null ? 0 : state.Id,                              
                              cityName = m.CityName == null ? "" : m.CityName,
                              stateName = state == null ? "" : state.stateName,                              
                              zipcode = m.ZipCode,
                              ImagePath = m.ImagePath,
                              NotificationEnable = m.NotificationEnable,
                              Timezone = detail == null ? "" : detail.Times,
                          }).FirstOrDefault();

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

        public async Task<Users> UpdateUserProfile(Users users)
        {
            try
            {
                Users ousers = _dbContext.Users.Find(users.Id);
                ousers.PhoneNumber = users.PhoneNumber;
                ousers.AddressLine1 = users.AddressLine1;
                ousers.AddressLine2 = users.AddressLine2;
                ousers.CityName = users.CityName;
                ousers.StateID = users.StateID;
                ousers.CountryID = users.CountryID;
                ousers.ZipCode = users.ZipCode;
                ousers.ModifiedBy = users.ModifiedBy;
                ousers.ModifiedOn = DateTime.Now;
                ousers.NotificationEnable = users.NotificationEnable;
                _dbContext.Update(ousers);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return (users);
        }
        public async Task<Users> UpdateUserPicture(Users users)
        {
            try
            {
                Users ousers = _dbContext.Users.FirstOrDefault(m => m.ObjectId == _tokenbase.getobjectid());
                ousers.ImagePath = users.ImagePath;
                _dbContext.Update(ousers);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return (users);
        }

        public async Task<bool> DeleteAdminOrUser(int userId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var allowedRoleIds = new List<long> { 3, 4 };
                // 3 = Admin, 4 = User

                var canDelete = await _dbContext.UserRoles
                    .AnyAsync(x => x.UserID == userId &&
                                   allowedRoleIds.Contains(x.RoleID));

                if (!canDelete)
                    return false;

                await _dbContext.UserRoles
                    .Where(x => x.UserID == userId)
                    .ExecuteDeleteAsync();

                var deleted = await _dbContext.Users
                    .Where(x => x.Id == userId)
                    .ExecuteDeleteAsync() > 0;

                await transaction.CommitAsync();

                return deleted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

       
    }
}
