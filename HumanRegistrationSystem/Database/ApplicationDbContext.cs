using HumanRegistrationSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanRegistrationSystem.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Account-Person 
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Person)
                .WithOne(p => p.Account)
                .HasForeignKey<Person>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // If Account is deleted, delete Person too

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId);
                
            // Person-Address 
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Person)
                .WithOne(p => p.Address)
                .HasForeignKey<Address>(a => a.PersonId)
                .OnDelete(DeleteBehavior.Cascade); // If Person is deleted, delete Address too

            // Person-ProfilePicture 
            modelBuilder.Entity<ProfilePicture>()
                .HasOne(pp => pp.Person)
                .WithOne(p => p.ProfilePicture)
                .HasForeignKey<ProfilePicture>(pp => pp.PersonId)
                .OnDelete(DeleteBehavior.Cascade); // If Person is deleted, delete ProfilePicture too

            modelBuilder.Entity<Account>()
                .Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Person>()
                .Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Address>()
                .Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProfilePicture>()
                .Property(pp => pp.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Admin" }
            );
        }
    }
}
