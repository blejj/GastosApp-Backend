using GastosApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GastosApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Gasto> Gastos => Set<Gasto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(c => c.Nombre).HasMaxLength(60).IsRequired();
            entity.HasOne(c => c.Usuario)
                  .WithMany(u => u.Categorias)
                  .HasForeignKey(c => c.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.Property(g => g.Monto).HasColumnType("decimal(18,2)");
            entity.Property(g => g.Descripcion).HasMaxLength(200);

            entity.HasOne(g => g.Usuario)
                  .WithMany(u => u.Gastos)
                  .HasForeignKey(g => g.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Categoria)
                  .WithMany(c => c.Gastos)
                  .HasForeignKey(g => g.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(g => new { g.UsuarioId, g.Fecha });
        });

        base.OnModelCreating(modelBuilder);
    }
}
