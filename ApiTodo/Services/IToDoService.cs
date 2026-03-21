using ApiTodo.DTOs;
using ApiTodo.Models;

namespace ApiTodo.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDoItemDto>> GetAllByUserAsync(int userId);
        Task<ToDoItemDto?> GetByIdAsync(int id, int userId);
        Task<ToDoItemDto> CreateAsync(ToDoItemCreateDto createDto, int userId);
        Task<bool> UpdateAsync(int id, ToDoItemUpdateDto updateDto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
