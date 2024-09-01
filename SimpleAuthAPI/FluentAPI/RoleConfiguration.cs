using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Entities;

namespace SimpleAuthAPI.FluentAPI
{
    // Configuration class for the Role entity using Fluent API
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Define the table name for the Role entity
            builder.ToTable("Roles");

            // Seed initial data for the Roles table
            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin", // Role name
                    NormalizedName = "ADMIN" 
                },
                new Role
                {
                    Id = 2,
                    Name = "User", // Role name
                    NormalizedName = "USER" 
                }
            );
        }
    }
}
