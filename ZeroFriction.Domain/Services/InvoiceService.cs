using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Persistance;
using ZeroFriction.Persistance.Entities;

namespace ZeroFriction.Domain.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ZeroFrictionDBContext _dBContext;
        private readonly IMapper _mapper;

        public InvoiceService(ZeroFrictionDBContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<List<InvoiceDto>> GetInvoices()
        {
            var invoices = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .AsNoTracking()
                .ToListAsync();

            var response = new List<InvoiceDto>();

            invoices.ForEach(invoice =>
            {
                response.Add(_mapper.Map<InvoiceDto>(invoice));
            });
            return response;
        }

        public async Task<InvoiceDto> GetInvoice(long id)
        {
            var invoice = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<InvoiceDto> CreateInvoice(InvoiceDto invoiceToCreate)
        {
            Invoice invoice = _mapper.Map<Invoice>(invoiceToCreate);
            await _dBContext.AddAsync(invoice);

            await _dBContext.SaveChangesAsync();
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<InvoiceDto> UpdateInvoice(InvoiceDto invoiceToUpdate)
        {
            Invoice invoiceInDB = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .FirstOrDefaultAsync(i => i.Id == invoiceToUpdate.Id);

            if (invoiceInDB != null)
            {
                invoiceInDB.Date = invoiceToUpdate.Date;
                invoiceInDB.Description = invoiceToUpdate.Description;
                invoiceInDB.TotalAmount = invoiceToUpdate.TotalAmount;
                _dBContext.InvoiceLines.RemoveRange(invoiceInDB.InvoiceLines);

                foreach (var invoiceLine in invoiceToUpdate.InvoiceLines)
                {
                    var newInvoiceLine = new InvoiceLine()
                    {
                        InvoiceId = invoiceInDB.Id,
                        Amount = invoiceLine.Amount,
                        Quantity = invoiceLine.Quantity,
                        UnitPrice = invoiceLine.UnitPrice,
                        LineAmount = invoiceLine.LineAmount
                    };
                    await _dBContext.InvoiceLines.AddAsync(newInvoiceLine);
                }
                await _dBContext.SaveChangesAsync();
                return _mapper.Map<InvoiceDto>(invoiceInDB);
            }
            else
            {
                throw new Exception("UpdateInvoice: Couldn't find the invoice to update.");
            }
        }

        public async Task DeleteInvoice(long id)
        {
            Invoice invoiceInDB = await _dBContext.Invoices
               .Include(i => i.InvoiceLines)
               .FirstOrDefaultAsync(i => i.Id == id);

            if (invoiceInDB != null)
            {
                _dBContext.Invoices.Remove(invoiceInDB);
                await _dBContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("DeleteInvoice: Couldn't find the invoice to delete.");
            }
        }
    }
}
