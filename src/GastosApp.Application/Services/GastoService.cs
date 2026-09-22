using GastosApp.Application.DTOs;
using GastosApp.Application.Interfaces;
using GastosApp.Domain.Entities;

namespace GastosApp.Application.Services;

public class GastoService
{
    private readonly IGastoRepository _gastoRepository;

    public GastoService(IGastoRepository gastoRepository)
    {
        _gastoRepository = gastoRepository;
    }

    public async Task<(IEnumerable<GastoResponse> Items, int Total)> ListarAsync(Guid usuarioId, GastoFiltro filtro)
    {
        var (items, total) = await _gastoRepository.ListarAsync(
            usuarioId, filtro.Desde, filtro.Hasta, filtro.CategoriaId, filtro.Tipo,
            filtro.Pagina, filtro.TamanoPagina);

        var respuesta = items.Select(g => new GastoResponse(
            g.Id, g.Monto, g.Descripcion, g.Fecha, g.Tipo,
            g.CategoriaId, g.Categoria?.Nombre ?? string.Empty));

        return (respuesta, total);
    }

    public async Task<GastoResponse> CrearAsync(Guid usuarioId, GastoRequest request)
    {
        var gasto = new Gasto
        {
            Monto = request.Monto,
            Descripcion = request.Descripcion,
            Fecha = request.Fecha,
            Tipo = request.Tipo,
            CategoriaId = request.CategoriaId,
            UsuarioId = usuarioId
        };

        await _gastoRepository.AgregarAsync(gasto);
        await _gastoRepository.GuardarCambiosAsync();

        return new GastoResponse(gasto.Id, gasto.Monto, gasto.Descripcion, gasto.Fecha,
            gasto.Tipo, gasto.CategoriaId, string.Empty);
    }

    public async Task<GastoResponse?> ActualizarAsync(Guid usuarioId, Guid id, GastoRequest request)
    {
        var gasto = await _gastoRepository.ObtenerPorIdAsync(id, usuarioId);
        if (gasto is null) return null;

        gasto.Monto = request.Monto;
        gasto.Descripcion = request.Descripcion;
        gasto.Fecha = request.Fecha;
        gasto.Tipo = request.Tipo;
        gasto.CategoriaId = request.CategoriaId;

        _gastoRepository.Actualizar(gasto);
        await _gastoRepository.GuardarCambiosAsync();

        return new GastoResponse(gasto.Id, gasto.Monto, gasto.Descripcion, gasto.Fecha,
            gasto.Tipo, gasto.CategoriaId, gasto.Categoria?.Nombre ?? string.Empty);
    }

    public async Task<bool> EliminarAsync(Guid usuarioId, Guid id)
    {
        var gasto = await _gastoRepository.ObtenerPorIdAsync(id, usuarioId);
        if (gasto is null) return false;

        _gastoRepository.Eliminar(gasto);
        await _gastoRepository.GuardarCambiosAsync();
        return true;
    }
}
