using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroFriction.Domain.Dtos;

namespace ZeroFriction.Domain.Services
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetInvoices();
        Task<InvoiceDto> GetInvoice(long id);
        Task<InvoiceDto> CreateInvoice(InvoiceDto invoiceToCreate);
        Task<InvoiceDto> UpdateInvoice(InvoiceDto invoiceToUpdate);
        Task DeleteInvoice(long id);
    }
}
