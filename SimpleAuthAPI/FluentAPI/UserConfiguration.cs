using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAuthAPI.Entities;

namespace SimpleAuthAPI.FluentAPI
{
    // Configuration class for the User entity using Fluent API
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Define the table name for the User entity
            builder.ToTable("Users");

            // Set the primary key for the User entity
            builder.HasKey(p => p.Id);

            // Configure properties with specific column types and constraints
            builder.Property(p => p.FirstName)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasColumnType("varchar(100)")
                .IsRequired(); // Mark the property as required

            builder.Property(f => f.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(f => f.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.PasswordHash)
                .HasColumnName("Password") // Map to column name "Password"
                .HasColumnType("varchar(250)"); // Set the column type to varchar(250)

            builder.Property(p => p.PhoneNumber)
                .HasColumnType("varchar(15)");

            builder.Property(p => p.Email)
                .HasColumnType("varchar(100)"); // Set the column type to varchar(100)

            builder.Property(p => p.NormalizedEmail)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.UserName)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.NormalizedUserName)
                .HasColumnType("varchar(100)"); // 

            builder.Property(p => p.ConcurrencyStamp)
                .HasMaxLength(250) // Set the maximum length to 250 characters
                .HasColumnType("varchar");

            builder.Property(p => p.SecurityStamp)
                .HasColumnType("varchar(250)"); // Set the column type to varchar(250)

            // Seed initial data for the Users table
            builder.HasData(new User
            {
                Id = 1,
                FirstName = "Adedeji",
                LastName = "Adeyemi",
                Email = "admin@test.ng",
                NormalizedEmail = "admin@test.ng".ToUpper(),
                UserName = "admin@test.ng",
                NormalizedUserName = "admin@test.ng".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"), // Generate a new GUID for SecurityStamp
                PhoneNumber = "08031234567",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                LockoutEnd = null,
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAEAACcQAAAAEHz9jeDAGD5NrInBBafBqFjW3XbnNG4w08PuNblIMvwdU1kzpGQd8mX3ca28HlBPkA==" // Example hash (should be generated securely)
            });
        }
    }
}