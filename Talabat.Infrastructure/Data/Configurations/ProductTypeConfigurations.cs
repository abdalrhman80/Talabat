using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Domain.Models;

namespace Talabat.Infrastructure.Data.Configurations
{
    internal class ProductTypeConfigurations : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
