using GastosApp.Application.Interfaces;
using GastosApp.Domain.Entities;
using GastosApp.Domain.Enums;
using GastosApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosApp.Infrastructure.Repositories;

public class GastoRepository : IGastoRepository
{
    private readonly AppDbContext _context;

    public GastoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Gasto?> ObtenerPorIdAsync(Guid id, Guid usuarioId)
    {
        return await _context.Gastos
            .Include(g => g.Categoria)
            .FirstOrDefaultAsync(g => g.Id == id && g.UsuarioId == usuarioId);
    }

    public async Task<(IEnumerable<Gasto> Items, int Total)> ListarAsync(
        Guid usuarioId, DateTime? desde, DateTime? hasta, Guid? categoriaId,
        TipoMovimiento? tipo, int pagina, int tamanoPagina)
    {
        var query = _context.Gastos
            .Include(g => g.Categoria)
            .Where(g => g.UsuarioId == usuarioId);

        if (desde.HasValue) query = query.Where(g => g.Fecha >= desde.Value);
        if (hasta.HasValue) query = query.Where(g => g.Fecha <= hasta.Value);
        if (categoriaId.HasValue) query = query.Where(g => g.CategoriaId == categoriaId.Value);
        if (tipo.HasValue) query = query.Where(g => g.Tipo == tipo.Value);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(g => g.Fecha)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        return (items, total);
    }

    public async Task AgregarAsync(Gasto gasto) => await _context.Gastos.AddAsync(gasto);

    public void Actualizar(Gasto gasto) => _context.Gastos.Update(gasto);

    public void Eliminar(Gasto gasto) => _context.Gastos.Remove(gasto);

    public async Task<bool> GuardarCambiosAsync() => await _context.SaveChangesAsync() > 0;
}
