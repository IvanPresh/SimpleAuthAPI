using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Entities;
using System.Reflection;

namespace SimpleAuthAPI.Data
{
    
    public class MyDbContext : IdentityDbContext<User, Role, int>
    {
        // Constructor that accepts DbContextOptions and passes it to the base class
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        // Configures the model and applies custom configurations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);

            // Apply all configurations from the current assembly 
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

          
            var tableNameWithAspNet = builder.Model.GetEntityTypes()
                .Where(e => e.GetTableName().StartsWith("AspNet"))
                .ToList();

            tableNameWithAspNet.ForEach(x =>
            {
                // Remove the "AspNet" prefix from the table name
                x.SetTableName(x.GetTableName().Substring(6));
            });
        }
    }
}
