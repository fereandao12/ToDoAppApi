using ApiTodo.DTOs;

namespace ApiTodo.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> AuthenticateAsync(UserAuthDto authDto);
        Task<bool> RegisterAsync(UserAuthDto authDto);
    }
}
