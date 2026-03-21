using ApiTodo.Data;
using ApiTodo.DTOs;
using ApiTodo.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.Services
{
    // Servicio encargado de gestionar las operaciones CRUD de tareas por usuario.
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        // Inicializa el servicio inyectando el contexto de base de datos y AutoMapper.
        public ToDoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Crea una nueva tarea mapeando el DTO y la asocia al ID del usuario.
        public async Task<ToDoItemDto> CreateAsync(ToDoItemCreateDto createDto, int userId)
        {
            var entity = _mapper.Map<ToDoItem>(createDto);
            entity.UserId = userId;

            _context.toDoItems.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ToDoItemDto>(entity);
        }

        // Obtiene todas las tareas que pertenecen exclusivamente al usuario indicado.
        public async Task<IEnumerable<ToDoItemDto>> GetAllByUserAsync(int userId)
        {
            var items = await _context.toDoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ToDoItemDto>>(items);
        }

        // Busca una tarea específica por su ID, asegurando que el usuario sea el propietario.
        public async Task<ToDoItemDto?> GetByIdAsync(int id, int userId)
        {
            var item = await _context.toDoItems
                .FirstOrDefaultAsync(t => t.id == id && t.UserId == userId);
            return item == null ? null : _mapper.Map<ToDoItemDto>(item);
        }

        // Actualiza los datos de una tarea existente si se valida la propiedad del usuario.
        public async Task<bool> UpdateAsync(int id, ToDoItemUpdateDto updateDto, int userId)
        {
            var entity = await _context.toDoItems.FirstOrDefaultAsync(t => t.id == id && t.UserId == userId);
            if (entity == null) return false;

            _mapper.Map(updateDto, entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // Elimina una tarea de la base de datos previa validación de que pertenezca al usuario.
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var entity = await _context.toDoItems.FirstOrDefaultAsync(t => t.id == id && t.UserId == userId);
            if (entity == null) return false;

            _context.toDoItems.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}