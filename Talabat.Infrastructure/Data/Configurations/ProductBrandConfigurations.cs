using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data.Configurations
{
    internal class ProductBrandConfigurations : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
                builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);
        }
    }
}
