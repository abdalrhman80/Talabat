using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data.Configurations
{
    public class WishListItemConfiguration : IEntityTypeConfiguration<WishListItem>
    {
        public void Configure(EntityTypeBuilder<WishListItem> builder)
        {
            builder.Property(w => w.UserEmail)
                   .HasMaxLength(256);

            builder.HasOne(w => w.Product)
                   .WithMany()
                   .HasForeignKey(w => w.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // one product per user in wishlist
            builder.HasIndex(w => new { w.UserEmail, w.ProductId })
                   .IsUnique();
        }
    }
}
