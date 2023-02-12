using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFriction.Persistance.Entities;

namespace ZeroFriction.Persistance.EntityConfigurations
{
    public class InvoiceLineEntityConfiguration : IEntityTypeConfiguration<InvoiceLine>
    {
        public void Configure(EntityTypeBuilder<InvoiceLine> builder)
        {
            builder.ToTable("InvoiceLine");

            builder.HasKey(il => il.Id);

            builder.HasOne(il => il.Invoice)
                .WithMany(i => i.InvoiceLines)
                .HasForeignKey(il => il.InvoiceId);
        }
    }
}
