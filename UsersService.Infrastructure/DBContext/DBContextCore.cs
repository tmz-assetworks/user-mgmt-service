using Microsoft.EntityFrameworkCore;
namespace UsersService.Infrastructure.DBContext
{
    public class DBContextCore : DbContext
    {
        public DBContextCore(DbContextOptions<DBContextCore> options) : base(options)
        {

        }
        public DbSet<UsersService.Core.Entities.Country> Country { get; set; }
        public DbSet<UsersService.Core.Entities.Customers> Customers { get; set; }
        public DbSet<UsersService.Core.Entities.Roles> Roles { get; set; }
        public DbSet<UsersService.Core.Entities.State> State { get; set; }
        public DbSet<UsersService.Core.Entities.UserRoles> UserRoles { get; set; }
        public DbSet<UsersService.Core.Entities.OperatorUserMapper> OperatorUserMapper { get; set; }
        public DbSet<UsersService.Core.Entities.Users> Users { get; set; }
        public DbSet<UsersService.Core.Entities.City> City { get; set; }
        public DbSet<UsersService.Core.Entities.Charger> Charger { get; set; }
        public DbSet<UsersService.Core.Entities.DispenserStatus> DispenserStatus { get; set; }
        public DbSet<UsersService.Core.Entities.Model> Model { get; set; }
        public DbSet<UsersService.Core.Entities.Modem> Modem { get; set; }
        public DbSet<UsersService.Core.Entities.Pad> Pads { get; set; }
        public DbSet<UsersService.Core.Entities.PowerCabinet> PowerCabinet { get; set; } 
        public DbSet<UsersService.Core.Entities.Cable> Cables { get; set; }
        public DbSet<UsersService.Core.Entities.SwitchGear> SwitchGears { get; set; }
        public DbSet<UsersService.Core.Entities.RFIDReader> RFIDReaders { get; set; }
        public DbSet<UsersService.Core.Entities.Location> Locations { get; set; }
        public DbSet<UsersService.Core.Entities.EmailTemplate> EmailTemplate { get; set; }
        public DbSet<UsersService.Core.Entities.SpecificTimeZone> TimeZones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UsersService.Core.Entities.SpecificTimeZone>(SpecificTimeZone =>
            {
                SpecificTimeZone.ToTable("SpecificTimeZone");

                SpecificTimeZone.Property(e => e.CreatedOn).HasColumnType("datetime").HasColumnName("CreatedOn");
                SpecificTimeZone.Property(e => e.IsActive).HasColumnName("IsActive");
            });
        }
    }
}
