using AutoMapper;
using FamilyAccountant.Application.Services.Family.Models;

namespace FamilyAccountant.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Family, FamilyDto>();
        CreateMap<Domain.Entities.User, FamilyMemberDto>()
            .ForMember(p => p.UserLogin, opt => opt.MapFrom(p => p.Login));
    }
}