using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFriction.Persistance.Entities;

namespace ZeroFriction.Persistance.EntityConfigurations
{
    public class InvoiceEntityConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoice");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description).HasMaxLength(250);
        }
    }
}
