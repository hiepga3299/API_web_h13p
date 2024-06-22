using API.web_h13p.Application.DTO.UserDTO;
using API.web_h13p.Domain.Entities;
using AutoMapper;

namespace API.web_h13p.Application.Configuration;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRegisterRequestDTO, User>();
    }
}