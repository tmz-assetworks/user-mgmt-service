using UsersService.Core.Entities;
using UsersService.Core.Repositories.Base;

namespace UsersService.Core.Repositories.Users
{
    /// </summary>
    public interface IUserRepository : IRepository<UsersService.Core.Entities.Users>
    {
        //custom operations here
        Task<IEnumerable<UsersService.Core.Entities.Users>> GetUserByEmailIdPassword(string emailId, string password);

        Task<List<UsersService.Core.Entities.Users>> GetAllUsers();
        Task<UsersService.Core.Entities.Users> GetByIdUser(long id);
        Task<UsersService.Core.Entities.Users> GetByObjectIdUser(string objectid);
        Task<UsersService.Core.Entities.Users> GetUserPrincipalNameUser(string userPrincipalName);
    }
}
