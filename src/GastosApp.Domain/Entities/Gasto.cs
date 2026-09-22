using GastosApp.Domain.Enums;

namespace GastosApp.Domain.Entities;

public class Gasto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
    public DateTime Fecha { get; set; }
    public TipoMovimiento Tipo { get; set; }

    public Guid CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public Guid UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
