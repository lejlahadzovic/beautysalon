using AutoMapper;
using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<UserVM, User>();
            CreateMap<CatalogVM, Catalog>().ReverseMap();
            CreateMap<ServiceVM, Service>().ReverseMap();
            CreateMap<UserUpdateVM, User>().ReverseMap();
            CreateMap<AppointmentVM, Appointment>().ReverseMap();

            CreateMap<User, UserVM>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.FirstName, y => y.MapFrom(z => z.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(z => z.LastName))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(z => z.PhoneNumber))
                .ForMember(x => x.Gender, y => y.MapFrom(z => z.Gender))
                .ForMember(x => x.BirthDate, y => y.MapFrom(z => z.BirthDate))
                .ForMember(x => x.Photo, y => y.MapFrom(z => z.Photo))
                .ForMember(x => x.FullName, y => y.MapFrom(z => z.FirstName + " " + z.LastName));
        }
    }
}
