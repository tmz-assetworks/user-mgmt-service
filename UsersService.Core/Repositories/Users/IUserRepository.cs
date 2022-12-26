using UsersService.Core.Repositories.Base;
using UsersService.Core.Response;

namespace UsersService.Core.Repositories.Users
{
    /// </summary>
    public interface IUserRepository : IRepository<UsersService.Core.Entities.Users>
    {
        //custom operations here
        Task<IEnumerable<UsersService.Core.Entities.Users>> GetUserByEmailIdPassword(string emailId, string password);

        Task<AllUserResponse> GetAllUsers(GetUserRequest getUserRequest);
        Task<GetUserResponseDT> GetByIdUser(long id);
        Task<UsersService.Core.Entities.Users> GetByObjectIdUser(string objectid);
        Task<UsersService.Core.Entities.Users> GetUserPrincipalNameUser(string userPrincipalName);
        Task<UpdateUserResponse> UpdateUser(UsersService.Core.Entities.Users users);
        Task<otpdata> Updateotp(string Emailid, string Otp);
        Task<otpdata> Getotp(string userid,string Otp);
        Task<GetUserobjectbyidDT> GetUserbyobjectId(string userid);
        Task<List<CustomerData>> GetCustomerDDL();
        Task<List<userrolesDDL>> GetUserDDL();
        Task<EmailResponse> GetUserEmail(string Useremail);
        Task<GetUserProfileResponseDT> GetByProfileUser();
        Task<UsersService.Core.Entities.Users> UpdateUserPicture(UsersService.Core.Entities.Users users);
        Task<UsersService.Core.Entities.Users> UpdateUserProfile(UsersService.Core.Entities.Users users);
    }
}
