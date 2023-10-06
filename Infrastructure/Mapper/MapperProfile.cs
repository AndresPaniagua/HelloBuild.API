using AutoMapper;
using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;

namespace HelloBuild.Infrastructure.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            _ = CreateMap<UserRequest, User>();
        }
    }
}
