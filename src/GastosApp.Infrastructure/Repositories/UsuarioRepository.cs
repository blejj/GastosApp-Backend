using GastosApp.Application.Interfaces;
using GastosApp.Domain.Entities;
using GastosApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosApp.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Usuario?> ObtenerPorEmailAsync(string email) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public Task<bool> ExisteEmailAsync(string email) =>
        _context.Usuarios.AnyAsync(u => u.Email == email);

    public async Task AgregarAsync(Usuario usuario) => await _context.Usuarios.AddAsync(usuario);

    public async Task<bool> GuardarCambiosAsync() => await _context.SaveChangesAsync() > 0;
}
