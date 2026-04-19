using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data.Configurations
{
    internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.ShortName)
                   .HasMaxLength(100);

            builder.Property(m => m.Description)
                   .HasMaxLength(500);

            builder.Property(m => m.DeliveryTime)
                   .HasMaxLength(100);

            builder.Property(m => m.Cost)
                   .HasColumnType("decimal(10,2)");

            builder.HasData(
                new DeliveryMethod
                {
                    Id = 1,
                    ShortName = "Express",
                    Description = "Fastest delivery time",
                    DeliveryTime = "1-2 Days",
                    Cost = 75.00m,
                },
                new DeliveryMethod
                {
                    Id = 2,
                    ShortName = "Standard",
                    Description = "Get it within 5 days",
                    DeliveryTime = "2-5 Days",
                    Cost = 50.00m,
                },
                new DeliveryMethod
                {
                    Id = 3,
                    ShortName = "Economy",
                    Description = "Slower but cheap",
                    DeliveryTime = "5-10 Days",
                    Cost = 25.00m,
                },
                new DeliveryMethod
                {
                    Id = 4,
                    ShortName = "Free Shipping",
                    Description = "Free! You get what you pay for",
                    DeliveryTime = "1-2 Weeks",
                    Cost = 0.00m,
                }
            );
        }
    }
}
