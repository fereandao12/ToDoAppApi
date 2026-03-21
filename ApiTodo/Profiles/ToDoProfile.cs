using ApiTodo.DTOs;
using ApiTodo.Models;
using ApiTodo.Models.Enums;
using AutoMapper;

namespace ApiTodo.Profiles
{
    // Perfil de AutoMapper que define las reglas de transformación entre la entidad ToDoItem y sus respectivos DTOs.
    public class ToDoProfile : Profile
    {
        // Configura los mapeos, gestionando la conversión de estados (enums) y asignando valores iniciales por defecto.
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