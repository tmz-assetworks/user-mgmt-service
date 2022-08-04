using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;


namespace UsersService.Infrastructure.DBContext
{
    public class DBContextCore : DbContext
    {
        public DBContextCore(DbContextOptions<DBContextCore> options) : base(options)
        {

        }

        public DbSet<UsersService.Core.Entities.Countries> Countries { get; set; }
        public DbSet<UsersService.Core.Entities.Customers> Customers { get; set; }
        public DbSet<UsersService.Core.Entities.Roles> Roles { get; set; }
        public DbSet<UsersService.Core.Entities.States> States { get; set; }
        public DbSet<UsersService.Core.Entities.UserRoles> UserRoles { get; set; }

        public DbSet<UsersService.Core.Entities.Users> Users { get; set; }

    }
}
