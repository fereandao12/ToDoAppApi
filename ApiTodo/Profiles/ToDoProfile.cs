using ApiTodo.DTOs;
using ApiTodo.Models;
using ApiTodo.Models.Enums;
using AutoMapper;

namespace ApiTodo.Profiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<ToDoItem, ToDoItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<ToDoItemCreateDto, ToDoItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ToDoStatus.Pendiente))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<ToDoItemUpdateDto, ToDoItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ToDoStatus)src.StatusId));
        }
    }
}
