using System;
using System.Collections.Generic;

namespace ZeroFriction.Persistance.Entities
{
    public class Invoice
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual List<InvoiceLine> InvoiceLines { get; set; }
    }
}
