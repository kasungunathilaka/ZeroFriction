using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Domain.Mappers;
using ZeroFriction.Domain.Services;
using ZeroFriction.Persistance;
using ZeroFriction.Persistance.Entities;

namespace ZeroFriction.Domain.Tests
{
    public class InvoiceServiceTests
    {
        private readonly ZeroFrictionDBContext _dBContext;
        private readonly IMapper _mapper;

        public InvoiceServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new InvoiceMapper());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var dbBuilder = new DbContextOptionsBuilder<ZeroFrictionDBContext>();
            var options = dbBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _dBContext = new ZeroFrictionDBContext(options);
        }

        internal InvoiceService InvoiceService
        {
            get
            {
                return new InvoiceService(_dBContext, _mapper);
            }
        }

        [Fact]
        public async Task GetInvoices_Success_ReturnsInvoices()
        {
            var invoice1 = new Invoice()
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Invoice 1",
                TotalAmount = 30,
                InvoiceLines = new List<InvoiceLine>()
                {
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 10,
                        LineAmount = 10
                    },
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 20,
                        LineAmount = 20
                    }
                }
            };
            var invoice2 = new Invoice()
            {
                Id = 2,
                Date = DateTime.Now,
                Description = "Test Invoice 2",
                TotalAmount = 80,
                InvoiceLines = new List<InvoiceLine>()
                {
                    new InvoiceLine()
                    {
                        Amount = 2,
                        Quantity = 2,
                        UnitPrice = 30,
                        LineAmount = 60
                    },
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 20,
                        LineAmount = 20
                    }
                }
            };
            await _dBContext.AddRangeAsync(new List<Invoice>() { invoice1, invoice2 });
            await _dBContext.SaveChangesAsync();

            var invoices = await InvoiceService.GetInvoices();
            Assert.Equal(await _dBContext.Invoices.AsNoTracking().CountAsync(), invoices.Count);
        }

        [Fact]
        public async Task GetInvoice_Success_ReturnsInvoice()
        {
            var invoice1 = new Invoice()
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Invoice 1",
                TotalAmount = 30,
                InvoiceLines = new List<InvoiceLine>()
                {
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 10,
                        LineAmount = 10
                    },
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 20,
                        LineAmount = 20
                    }
                }
            };
            await _dBContext.AddRangeAsync(new List<Invoice>() { invoice1 });
            await _dBContext.SaveChangesAsync();

            var invoice = await InvoiceService.GetInvoice(1);
            var expectedInvoice = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == 1);

            Assert.Equal(expectedInvoice.Id, invoice.Id);
            Assert.Equal(expectedInvoice.Date, invoice.Date);
            Assert.Equal(expectedInvoice.Description, invoice.Description);
            Assert.Equal(expectedInvoice.TotalAmount, invoice.TotalAmount);
            Assert.Equal(expectedInvoice.InvoiceLines.Count, invoice.InvoiceLines.Count);
        }

        [Fact]
        public async Task CreateInvoice_Success_ReturnsInvoice()
        {
            var expectedInvoice = new InvoiceDto()
            {
                Date = DateTime.Now,
                Description = "Test Invoice 1",
                TotalAmount = 30,
                InvoiceLines = new List<InvoiceLineDto>()
                {
                    new InvoiceLineDto()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 10,
                        LineAmount = 10
                    },
                    new InvoiceLineDto()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 20,
                        LineAmount = 20
                    }
                }
            };

            var createdInvoice = await InvoiceService.CreateInvoice(expectedInvoice);

            Assert.Equal(1, createdInvoice.Id);
            Assert.Equal(expectedInvoice.Date, createdInvoice.Date);
            Assert.Equal(expectedInvoice.Description, createdInvoice.Description);
            Assert.Equal(expectedInvoice.TotalAmount, createdInvoice.TotalAmount);
            Assert.Equal(expectedInvoice.InvoiceLines.Count, createdInvoice.InvoiceLines.Count);
        }

        [Fact]
        public async Task UpdateInvoice_Success_ReturnsInvoice()
        {
            var invoice1 = new Invoice()
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Invoice 1",
                TotalAmount = 30,
                InvoiceLines = new List<InvoiceLine>()
                {
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 10,
                        LineAmount = 10
                    },
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 20,
                        LineAmount = 20
                    }
                }
            };
            await _dBContext.AddRangeAsync(new List<Invoice>() { invoice1 });
            await _dBContext.SaveChangesAsync();

            var expectedInvoice = new InvoiceDto()
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Invoice 1 updated",
                TotalAmount = 250,
                InvoiceLines = new List<InvoiceLineDto>()
                {
                    new InvoiceLineDto()
                    {
                        Amount = 5,
                        Quantity = 5,
                        UnitPrice = 50,
                        LineAmount = 250
                    }
                }
            };

            var invoiceBeforeUpdate = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == 1);

            var updatedInvoice = await InvoiceService.UpdateInvoice(expectedInvoice);

            Assert.Equal(expectedInvoice.Id, updatedInvoice.Id);
            Assert.Equal(expectedInvoice.Date, updatedInvoice.Date);
            Assert.Equal(expectedInvoice.Description, updatedInvoice.Description);
            Assert.Equal(expectedInvoice.TotalAmount, updatedInvoice.TotalAmount);
            Assert.Equal(expectedInvoice.InvoiceLines.Count, updatedInvoice.InvoiceLines.Count);
            Assert.NotEqual(expectedInvoice.Date, invoiceBeforeUpdate.Date);
            Assert.NotEqual(expectedInvoice.Description, invoiceBeforeUpdate.Description);
            Assert.NotEqual(expectedInvoice.TotalAmount, invoiceBeforeUpdate.TotalAmount);
            Assert.NotEqual(expectedInvoice.InvoiceLines.Count, invoiceBeforeUpdate.InvoiceLines.Count);
        }

        [Fact]
        public async Task UpdateInvoice_InvoiceNotFound_ThrowsException()
        {
            var expectedInvoice = new InvoiceDto()
            {
                Id = 210,
                Date = DateTime.Now,
                Description = "Test Invoice 1 updated",
                TotalAmount = 250,
                InvoiceLines = new List<InvoiceLineDto>()
                {
                    new InvoiceLineDto()
                    {
                        Amount = 5,
                        Quantity = 5,
                        UnitPrice = 50,
                        LineAmount = 250
                    }
                }
            };

            await Assert.ThrowsAsync<Exception>(() => InvoiceService.UpdateInvoice(expectedInvoice));
        }

        [Fact]
        public async Task DeleteInvoice_Success_RemovesInvoice()
        {
            var invoice1 = new Invoice()
            {
                Id = 1,
                Date = DateTime.Now,
                Description = "Test Invoice 1",
                TotalAmount = 30,
                InvoiceLines = new List<InvoiceLine>()
                {
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 10,
                        LineAmount = 10
                    },
                    new InvoiceLine()
                    {
                        Amount = 1,
                        Quantity = 1,
                        UnitPrice = 20,
                        LineAmount = 20
                    }
                }
            };
            await _dBContext.AddRangeAsync(new List<Invoice>() { invoice1 });
            await _dBContext.SaveChangesAsync();

            var invoiceBeforeDelete = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == 1);

            Assert.Equal(1, invoiceBeforeDelete.Id);
            Assert.Equal(1, await _dBContext.Invoices.AsNoTracking().CountAsync());
            Assert.Equal(2, await _dBContext.InvoiceLines.AsNoTracking().CountAsync());

            await InvoiceService.DeleteInvoice(1);
            var invoiceAfterDelete = await _dBContext.Invoices
                .Include(i => i.InvoiceLines)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == 1);

            Assert.Null(invoiceAfterDelete);
            Assert.Equal(0, await _dBContext.Invoices.AsNoTracking().CountAsync());
            Assert.Equal(0, await _dBContext.InvoiceLines.AsNoTracking().CountAsync());
        }

        [Fact]
        public async Task DeleteInvoice_InvoiceNotFound_ThrowsException()
        {
            await Assert.ThrowsAsync<Exception>(() => InvoiceService.DeleteInvoice(150));
        }
    }
}