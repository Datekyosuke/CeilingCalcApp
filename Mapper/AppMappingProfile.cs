using AutoMapper;
using CeilingCalc.Data.DTO_Material;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Models;

namespace WebApiDB.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<DealerDTOGet, Dealer>().ReverseMap();
            CreateMap<Order, OrderG>();
            CreateMap<Material, MaterialDTO>().ReverseMap();
        }
    }
}
