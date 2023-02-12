using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Domain.Services;

namespace ZeroFriction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetInvoices();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInvoices: Error occurred while invoking GetInvoices method.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(long id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoice(id);
                if (invoice != null)
                {
                    return Ok(invoice);
                }
                return NotFound("GetInvoice: Couldn't find the invoice.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInvoice: Error occurred while invoking GetInvoice method.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto invoiceToCreate)
        {
            try
            {
                var invoice = await _invoiceService.CreateInvoice(invoiceToCreate);
                return StatusCode(StatusCodes.Status201Created, invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateInvoice: Error occurred while invoking CreateInvoice method.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInvoice([FromBody] InvoiceDto invoiceToUpdate)
        {
            try
            {
                var invoice = await _invoiceService.UpdateInvoice(invoiceToUpdate);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInvoice: Error occurred while invoking UpdateInvoice method.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(long id)
        {
            try
            {
                await _invoiceService.DeleteInvoice(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteInvoice: Error occurred while invoking DeleteInvoice method.");
                return BadRequest(ex.Message);
            }
        }
    }
}