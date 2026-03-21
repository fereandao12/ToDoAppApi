using ApiTodo.DTOs;
using ApiTodo.Models;

namespace ApiTodo.Services
{
    // Define el contrato para las operaciones CRUD de las tareas de un usuario.
    public interface IToDoService
    {
        // Obtiene todas las tareas que pertenecen exclusivamente al usuario indicado.
        Task<IEnumerable<ToDoItemDto>> GetAllByUserAsync(int userId);

        // Busca una tarea específica por su ID, asegurando que el usuario sea el propietario.
        Task<ToDoItemDto?> GetByIdAsync(int id, int userId);

        // Crea una nueva tarea y la asocia al ID del usuario proporcionado.
        Task<ToDoItemDto> CreateAsync(ToDoItemCreateDto createDto, int userId);

        // Actualiza los datos de una tarea existente si se valida la propiedad del usuario.
        Task<bool> UpdateAsync(int id, ToDoItemUpdateDto updateDto, int userId);

        // Elimina una tarea de la base de datos previa validación de que pertenezca al usuario.
        Task<bool> DeleteAsync(int id, int userId);
    }
}
