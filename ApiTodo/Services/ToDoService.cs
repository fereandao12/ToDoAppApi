using ApiTodo.Data;
using ApiTodo.DTOs;
using ApiTodo.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ToDoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        

   
        public async Task<ToDoItemDto> CreateAsync(ToDoItemCreateDto createDto, int userId)
        {
            var entity = _mapper.Map<ToDoItem>(createDto);
            entity.UserId = userId;

            _context.toDoItems.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ToDoItemDto>(entity);
        }
        public async Task<IEnumerable<ToDoItemDto>> GetAllByUserAsync(int userId)
        {
            var items = await _context.toDoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ToDoItemDto>>(items);
        }

        public async Task<ToDoItemDto?> GetByIdAsync(int id, int userId)
        {
            var item = await _context.toDoItems
                .FirstOrDefaultAsync(t => t.id == id && t.UserId == userId);
            return item == null ? null : _mapper.Map<ToDoItemDto>(item);
        }

        public async Task<bool> UpdateAsync(int id, ToDoItemUpdateDto updateDto, int userId)
        {
            var entity = await _context.toDoItems.FirstOrDefaultAsync(t => t.id == id && t.UserId == userId);
            if (entity == null) return false;

            _mapper.Map(updateDto, entity);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var entity = await _context.toDoItems.FirstOrDefaultAsync(t => t.id == id && t.UserId == userId);
            if(entity == null) return false;

            _context.toDoItems.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
