using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProductType)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.ProductTypeId);

            builder.HasOne(p => p.ProductBrand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.ProductBrandId);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.HasMany(p => p.ProductPictures)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.StockQuantity)
                .HasDefaultValue(0);

            builder.Property(p => p.IsActive)
                .HasDefaultValue(false);
        }
    }
}
