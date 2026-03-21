using ApiTodo.DTOs;

namespace ApiTodo.Services
{
    // Define el contrato para gestionar el registro y la autenticación de usuarios mediante tokens JWT.
    public interface IAuthService
    {
        // Valida las credenciales del usuario y, si son correctas, genera y retorna un token JWT firmado.
        Task<AuthResponseDto?> AuthenticateAsync(UserAuthDto authDto);

        // Registra un nuevo usuario verificando que el nombre no exista y guardando su contraseña encriptada.
        Task<bool> RegisterAsync(UserAuthDto authDto);
    }
}
