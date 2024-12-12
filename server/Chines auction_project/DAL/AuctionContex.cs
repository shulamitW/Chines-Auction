using Chines_auction_project.Modells;
using Microsoft.EntityFrameworkCore;

namespace Chines_auction_project.DAL
{
    public class AuctionContex:DbContext
    {
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Donor> Donor { get; set; }
        public DbSet<Present> Present { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<Category> Category { get; set; }


        public AuctionContex(DbContextOptions<AuctionContex> contextOptions):base(contextOptions){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().Property(c => c.Id).UseIdentityColumn(1, 1);

            modelBuilder.Entity<Present>().Property(c => c.Id).UseIdentityColumn(1, 1);

            modelBuilder.Entity<Purchase>().Property(c => c.Id).UseIdentityColumn(1, 1);

            //modelBuilder.Entity<Donor>().Property(c => c.Id).UseIdentityColumn(1, 1);

            modelBuilder.Entity<Manager>().Property(c => c.Id).UseIdentityColumn(1, 1);

            modelBuilder.Entity<Category>().Property(c => c.Id).UseIdentityColumn(1, 1);

            modelBuilder.Entity<Ticket>().Property(c => c.Id).UseIdentityColumn(1, 1);

            base.OnModelCreating(modelBuilder);


        }
    }
}
