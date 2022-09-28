using UsersService.Core.Entities;
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
        Task<UsersService.Core.Entities.Users> UpdateUser(UsersService.Core.Entities.Users users);
        Task<GetUserobjectbyidDT> GetUserbyobjectId(string userid);
        Task<List<CustomerData>> GetCustomerDDL();
        Task<List<userrolesDDL>> GetUserDDL();
    }
}
