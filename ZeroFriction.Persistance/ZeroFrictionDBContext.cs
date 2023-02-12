using Microsoft.EntityFrameworkCore;
using ZeroFriction.Persistance.Entities;

namespace ZeroFriction.Persistance
{
    public class ZeroFrictionDBContext : DbContext
    {
        public ZeroFrictionDBContext(DbContextOptions<ZeroFrictionDBContext> options) : base(options) { }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
