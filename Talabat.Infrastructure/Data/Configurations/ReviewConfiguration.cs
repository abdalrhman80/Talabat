using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.BuyerEmail)
                    .HasMaxLength(256);

            builder.Property(r => r.BuyerName)
                    .HasMaxLength(100);

            builder.Property(r => r.Rating)
                   .HasColumnType("decimal(2,1)"); // supports 1.0 - 5.0

            builder.Property(r => r.Comment)
                   .HasMaxLength(1000);

            builder.HasOne(r => r.Product)
                   .WithMany(p => p.Reviews)
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // one review per user per product
            builder.HasIndex(r => new { r.BuyerEmail, r.ProductId })
                   .IsUnique();
        }
    }
}