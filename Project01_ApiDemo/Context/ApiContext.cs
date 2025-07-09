using Microsoft.EntityFrameworkCore;
using Project01_ApiDemo.Entities;

namespace Project01_ApiDemo.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-AGVT0F7;initial catalog=ApiAIDb;integrated security=true;trustservercertificate=true");
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
