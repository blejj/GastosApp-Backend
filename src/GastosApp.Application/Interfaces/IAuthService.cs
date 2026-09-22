using GastosApp.Application.DTOs;

namespace GastosApp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegistrarAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
