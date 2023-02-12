namespace ZeroFriction.Persistance.Entities
{
    public class InvoiceLine
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineAmount { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
