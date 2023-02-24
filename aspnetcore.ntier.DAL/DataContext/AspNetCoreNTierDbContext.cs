using aspnetcore.ntier.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore.ntier.DAL.DataContext;

public class AspNetCoreNTierDbContext : DbContext
{
    public AspNetCoreNTierDbContext(DbContextOptions<AspNetCoreNTierDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
             new User
             {
                 UserId = 1,
                 Username = "johndoe",
                 Password = "123",
                 Name = "John",
                 Surname = "Doe",
             }
         );
    }
}