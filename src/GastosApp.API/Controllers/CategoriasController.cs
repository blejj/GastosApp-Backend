using System.Security.Claims;
using GastosApp.Domain.Entities;
using GastosApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GastosApp.API.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    private Guid UsuarioId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _context.Categorias
            .Where(c => c.UsuarioId == UsuarioId)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] string nombre)
    {
        var categoria = new Categoria { Nombre = nombre, UsuarioId = UsuarioId };
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Listar), new { id = categoria.Id }, categoria);
    }
}
