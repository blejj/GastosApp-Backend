using GastosApp.Domain.Entities;

namespace GastosApp.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task<bool> ExisteEmailAsync(string email);
    Task AgregarAsync(Usuario usuario);
    Task<bool> GuardarCambiosAsync();
}
