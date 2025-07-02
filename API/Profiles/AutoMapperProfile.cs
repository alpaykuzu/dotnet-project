using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, LoginResponse>().
                ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.Token = context.Items["Token"] as String ?? "";
                    dest.RefreshToken = context.Items["RefreshToken"] as String ?? "";
                });
        }
    }
}
