using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Infrastructure.Data.Configurations
{
    public class RefundRequestConfiguration : IEntityTypeConfiguration<RefundRequest>
    {
        public void Configure(EntityTypeBuilder<RefundRequest> builder)
        {
            builder.Property(r => r.BuyerEmail)
                   .HasMaxLength(256);

            builder.Property(r => r.Reason)
                   .HasMaxLength(1000);

            builder.Property(r => r.Status)
                   .HasConversion(
                        statusApp => statusApp.ToString(),
                        statusDb => Enum.Parse<RefundRequestStatus>(statusDb)
                        );

            builder.Property(r => r.AdminNotes)
                   .HasMaxLength(1000);

            builder.HasOne(r => r.Order)
                   .WithOne()
                   .HasForeignKey<RefundRequest>(r => r.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            // one refund request per order
            builder.HasIndex(r => r.OrderId)
                   .IsUnique();

            builder.HasIndex(r => r.BuyerEmail);

            builder.HasIndex(r => r.Status);
        }
    }
}