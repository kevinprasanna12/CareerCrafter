using CareerCrafter.DTOs;
namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<object?> LoginAsync(LoginDTO dto);
        Task<object> RegisterAsync(RegisterDTO dto);
    }
}
