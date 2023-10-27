using AutoMapper;

using WebApiDB.Models;

namespace WebApiDB.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<DTOOrder, Order>().ReverseMap();
            CreateMap<DTODealer, Dealer>().ReverseMap();

        }
    }
}
