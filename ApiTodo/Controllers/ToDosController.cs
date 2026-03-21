using ApiTodo.DTOs;
using ApiTodo.Models;
using ApiTodo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiTodo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _service;

        public ToDosController(IToDoService service)
        {
            _service = service;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllByUserAsync(GetCurrentUserId());
            return Ok(items);
        }
        [HttpGet("{id}", Name = "GetTodoById")]
        public async Task<IActionResult>GetById(int id)
        {
            var item = await _service.GetByIdAsync(id, GetCurrentUserId());
            if(item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoItemCreateDto dto)
        {
            var createdItem = await _service.CreateAsync(dto, GetCurrentUserId());
            return CreatedAtRoute("GetTodoById", new { id = createdItem.Id }, createdItem);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]ToDoItemUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto, GetCurrentUserId());
            if(!success) return NotFound();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id, GetCurrentUserId());
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
