using AutoMapper;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Models;

namespace WebApiDB.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<DTODealer, Dealer>().ReverseMap();
            CreateMap<Order, OrderG>().ForMember(dest => dest.DealerFullName, opt => opt.MapFrom(src => $"{src.Dealer.FirstName} {src.Dealer.LastName}"));
        }
    }
}
