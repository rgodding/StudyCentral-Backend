using AutoMapper;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.ProfilePicture != null
                        ? src.ProfilePicture.BlobName
                        : null));
    }
}