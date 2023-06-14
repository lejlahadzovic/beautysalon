using AutoMapper;
using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<UserVM, User>().ReverseMap();
            CreateMap<CatalogVM, Catalog>().ReverseMap();
            CreateMap<ServiceVM, Service>().ReverseMap();
            CreateMap<UserUpdateVM, User>().ReverseMap();
        }
    }
}
