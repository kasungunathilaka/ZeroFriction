namespace ZeroFriction.Domain.Dtos
{
    public class InvoiceLineDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineAmount { get; set; }
    }
}
