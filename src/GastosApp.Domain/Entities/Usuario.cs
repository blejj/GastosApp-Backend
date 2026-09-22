namespace GastosApp.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
}
