using ApiTodo.DTOs;
using ApiTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.Controllers
{
    // Controlador que expone los endpoints públicos para el registro y autenticación de usuarios.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Inicializa el controlador inyectando el servicio de autenticación.
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Permite a cualquier usuario registrarse, validando que el nombre de usuario no esté en uso.
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserAuthDto dto)
        {
            var success = await _authService.RegisterAsync(dto);
            if (!success) return BadRequest("El usuario ya existe");

            return Ok("Usuario registrado correctamente");
        }

        // Permite a cualquier usuario iniciar sesión y retorna un token JWT si las credenciales son válidas.
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserAuthDto dto)
        {
            var result = await _authService.AuthenticateAsync(dto);
            if (result == null) return Unauthorized("Credenciales invalidas");

            return Ok(result);
        }
    }
}