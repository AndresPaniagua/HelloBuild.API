using AutoMapper;
using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;

namespace HelloBuild.Infrastructure.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            _ = CreateMap<LoanRequest, Prestamo>()
                .ForMember(loanR => loanR.Isbn, pres => pres.MapFrom(src => src.Isbn))
                .ForMember(loanR => loanR.UserId, pres => pres.MapFrom(src => src.IdentificacionUsuario))
                .ForMember(loanR => loanR.UserType, pres => pres.MapFrom(src => src.TipoUsuario))
                .ForMember(loanR => loanR.MaxDateReturn, pres => pres.MapFrom(src => src.TiempoDevolucion));

            _ = CreateMap<Prestamo, LoanUserResponse>()
               .ForMember(loanR => loanR.ReturnDate, pres => pres.MapFrom(src => src.MaxDateReturn));

        }
    }
}
