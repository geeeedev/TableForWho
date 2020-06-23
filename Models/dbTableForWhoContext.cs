
using Microsoft.EntityFrameworkCore;



namespace CsTableForWho.Models
{
    public class dbTableForWhoContext : DbContext
    {
        // context constructor using base constructor
        public dbTableForWhoContext(DbContextOptions options) : base(options) { }

        // tables in db
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<TableEntitySingular> TableNamePlural { get; set; } 
    }
}