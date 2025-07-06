using API.DTOs.Product;
using API.DTOs.User;
using API.DTOs.UserRole;
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
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.AccessTokenExpTime, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.AccessToken = context.Items["Token"] as String ?? "";
                    dest.RefreshToken = context.Items["RefreshToken"] as String ?? "";
                    dest.AccessTokenExpTime = context.Items["AccessTokenExpTime"] as String ?? "";
                    dest.RefreshTokenExpTime = context.Items["RefreshTokenExpTime"] as String ?? "";
                });

            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.PasswordHash = context.Items["PasswordHash"] as String ?? "";
                    //dest.Roles = (List<UserRole>)context.Items["UserRoles"];
                });

            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>();

            CreateMap<UserRoleRequest, UserRole>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
                /*.AfterMap((src, dest, context) =>
                {
                    dest.User = (User)context.Items["User"];
                });*/
            CreateMap<UserRole, UserRoleResponse>();
        


        }
    }
}
