using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;


namespace TestTask.Data;
public class AppDbContext : DbContext
{
    public DbSet<OrderModel> Orders {get;set;} = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

}