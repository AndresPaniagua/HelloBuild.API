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

            _ = CreateMap<User, GetUserInfoResponse>();

            _ = CreateMap<GitFavRepositoryResponse, GitRepositoryResponse>()
               .ForMember(repos => repos.SvnUrl, fav => fav.MapFrom(src => src.Svn_Url));
        }
    }
}
