using ApiTodo.DTOs;
using ApiTodo.Models;
using ApiTodo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiTodo.Controllers
{
    // Controlador que expone los endpoints de la API para gestionar las tareas del usuario autenticado.
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _service;

        // Inicializa el controlador inyectando el servicio de tareas.
        public ToDosController(IToDoService service)
        {
            _service = service;
        }

        // Extrae el ID del usuario actual a partir de los claims del token JWT.
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        // Retorna todas las tareas asociadas al usuario autenticado con un código 200 OK.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllByUserAsync(GetCurrentUserId());
            return Ok(items);
        }

        // Busca y retorna una tarea específica si pertenece al usuario actual, o 404 si no existe.
        [HttpGet("{id}", Name = "GetTodoById")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id, GetCurrentUserId());
            if (item == null) return NotFound();

            return Ok(item);
        }

        // Crea una nueva tarea para el usuario y retorna la ruta para consultarla junto con sus datos.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoItemCreateDto dto)
        {
            var createdItem = await _service.CreateAsync(dto, GetCurrentUserId());
            return CreatedAtRoute("GetTodoById", new { id = createdItem.Id }, createdItem);
        }

        // Actualiza una tarea existente validando la propiedad del usuario, retornando 204 si tiene éxito.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ToDoItemUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto, GetCurrentUserId());
            if (!success) return NotFound();

            return NoContent();
        }

        // Elimina una tarea validando que pertenezca al usuario, retornando 204 tras borrarla.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id, GetCurrentUserId());
            if (!success) return NotFound();

            return NoContent();
        }
    }
}