using System;
using System.Collections.Generic;

namespace ZeroFriction.Domain.Dtos
{
    public class InvoiceDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceLineDto> InvoiceLines { get; set; }
    }
}
