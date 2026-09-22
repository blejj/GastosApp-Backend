using GastosApp.Domain.Entities;

namespace GastosApp.Application.Interfaces;

public interface IGastoRepository
{
    Task<Gasto?> ObtenerPorIdAsync(Guid id, Guid usuarioId);
    Task<(IEnumerable<Gasto> Items, int Total)> ListarAsync(
        Guid usuarioId, DateTime? desde, DateTime? hasta, Guid? categoriaId,
        Domain.Enums.TipoMovimiento? tipo, int pagina, int tamanoPagina);
    Task AgregarAsync(Gasto gasto);
    void Actualizar(Gasto gasto);
    void Eliminar(Gasto gasto);
    Task<bool> GuardarCambiosAsync();
}
