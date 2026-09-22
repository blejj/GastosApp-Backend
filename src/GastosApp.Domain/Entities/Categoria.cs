namespace GastosApp.Domain.Entities;

public class Categoria
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string? Icono { get; set; }
    public string? ColorHex { get; set; }

    public Guid UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
