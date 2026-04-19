using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Infrastructure.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status).HasConversion(
                statusApp => statusApp.ToString(),
                statusDb => Enum.Parse<OrderStatus>(statusDb)
                );

            builder.Property(order => order.SubTotal)
                .HasColumnType("decimal(10,2)");

            builder.HasOne(o => o.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(o => o.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
