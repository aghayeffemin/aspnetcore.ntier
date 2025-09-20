using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DTO.Dtos;
using AutoMapper;

namespace aspnetcore.ntier.BLL.Utilities.AutoMapperProfiles;

public static class AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserToAddDto>().ReverseMap();
            CreateMap<User, UserToUpdateDto>().ReverseMap();
            CreateMap<User, UserToRegisterDto>().ReverseMap();
            CreateMap<User, UserToReturnDto>().ReverseMap();
        }
    }
}
