using GastosApp.Domain.Enums;

namespace GastosApp.Application.DTOs;

public record GastoRequest(
    decimal Monto,
    string? Descripcion,
    DateTime Fecha,
    TipoMovimiento Tipo,
    Guid CategoriaId
);

public record GastoResponse(
    Guid Id,
    decimal Monto,
    string? Descripcion,
    DateTime Fecha,
    TipoMovimiento Tipo,
    Guid CategoriaId,
    string CategoriaNombre
);

public record GastoFiltro(
    DateTime? Desde,
    DateTime? Hasta,
    Guid? CategoriaId,
    TipoMovimiento? Tipo,
    int Pagina = 1,
    int TamanoPagina = 20
);

public record ResumenMensual(int Anio, int Mes, decimal TotalIngresos, decimal TotalGastos, decimal Balance);

public record ResumenPorCategoria(Guid CategoriaId, string CategoriaNombre, decimal Total);
