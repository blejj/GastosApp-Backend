using GastosApp.Application.DTOs;
using GastosApp.Application.Interfaces;
using GastosApp.Domain.Entities;

namespace GastosApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUsuarioRepository usuarioRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegistrarAsync(RegisterRequest request)
    {
        if (await _usuarioRepository.ExisteEmailAsync(request.Email))
            throw new InvalidOperationException("Ya existe un usuario con ese email.");

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _usuarioRepository.AgregarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();

        var (token, expira) = _jwtTokenGenerator.GenerarToken(usuario);
        return new AuthResponse(token, expira, usuario.Nombre, usuario.Email);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(request.Email.ToLowerInvariant())
            ?? throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var (token, expira) = _jwtTokenGenerator.GenerarToken(usuario);
        return new AuthResponse(token, expira, usuario.Nombre, usuario.Email);
    }
}
