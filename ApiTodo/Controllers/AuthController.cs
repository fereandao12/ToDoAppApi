using ApiTodo.DTOs;
using ApiTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserAuthDto dto)
        {
            var success = await _authService.RegisterAsync(dto);
            if (!success) return BadRequest("El usuario ya existe");

            return Ok("Usuario registrado correctamente");
        }

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
