using System.Security.Claims;
using GastosApp.Application.DTOs;
using GastosApp.Application.Services;
using GastosApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GastosApp.API.Controllers;

[ApiController]
[Route("api/gastos")]
[Authorize]
public class GastosController : ControllerBase
{
    private readonly GastoService _gastoService;

    public GastosController(GastoService gastoService)
    {
        _gastoService = gastoService;
    }

    private Guid UsuarioId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta,
        [FromQuery] Guid? categoriaId, [FromQuery] TipoMovimiento? tipo,
        [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
    {
        var filtro = new GastoFiltro(desde, hasta, categoriaId, tipo, pagina, tamanoPagina);
        var (items, total) = await _gastoService.ListarAsync(UsuarioId, filtro);

        return Ok(new { total, pagina, tamanoPagina, items });
    }

    [HttpPost]
    public async Task<ActionResult<GastoResponse>> Crear(GastoRequest request)
    {
        var creado = await _gastoService.CrearAsync(UsuarioId, request);
        return CreatedAtAction(nameof(Listar), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GastoResponse>> Actualizar(Guid id, GastoRequest request)
    {
        var actualizado = await _gastoService.ActualizarAsync(UsuarioId, id, request);
        return actualizado is null ? NotFound() : Ok(actualizado);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        var eliminado = await _gastoService.EliminarAsync(UsuarioId, id);
        return eliminado ? NoContent() : NotFound();
    }
}
