using AutoMapper;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Persistance.Entities;

namespace ZeroFriction.Domain.Mappers
{
    public class InvoiceMapper : Profile
    {
        public InvoiceMapper()
        {
            CreateMap<InvoiceDto, Invoice>().ReverseMap();
            CreateMap<InvoiceLineDto, InvoiceLine>().ReverseMap();
        }
    }
}
