using System.Security.Claims;
using GastosApp.Application.DTOs;
using GastosApp.Domain.Enums;
using GastosApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GastosApp.API.Controllers;

[ApiController]
[Route("api/reportes")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportesController(AppDbContext context)
    {
        _context = context;
    }

    private Guid UsuarioId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("resumen-mensual")]
    public async Task<ActionResult<ResumenMensual>> ResumenMensual([FromQuery] int anio, [FromQuery] int mes)
    {
        var gastos = await _context.Gastos
            .Where(g => g.UsuarioId == UsuarioId && g.Fecha.Year == anio && g.Fecha.Month == mes)
            .ToListAsync();

        var ingresos = gastos.Where(g => g.Tipo == TipoMovimiento.Ingreso).Sum(g => g.Monto);
        var egresos = gastos.Where(g => g.Tipo == TipoMovimiento.Gasto).Sum(g => g.Monto);

        return Ok(new ResumenMensual(anio, mes, ingresos, egresos, ingresos - egresos));
    }

    [HttpGet("por-categoria")]
    public async Task<ActionResult<IEnumerable<ResumenPorCategoria>>> PorCategoria(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        var query = _context.Gastos
            .Include(g => g.Categoria)
            .Where(g => g.UsuarioId == UsuarioId && g.Tipo == TipoMovimiento.Gasto);

        if (desde.HasValue) query = query.Where(g => g.Fecha >= desde.Value);
        if (hasta.HasValue) query = query.Where(g => g.Fecha <= hasta.Value);

        var agrupado = await query
        .GroupBy(g => new { g.CategoriaId, g.Categoria!.Nombre })
        .Select(gr => new
        {
            gr.Key.CategoriaId,
            gr.Key.Nombre,
            Total = gr.Sum(x => x.Monto)
        })
        .OrderByDescending(x => x.Total)
        .ToListAsync();

            var resultado = agrupado.Select(x => new ResumenPorCategoria(x.CategoriaId, x.Nombre, x.Total));

            return Ok(resultado);
    }
}
