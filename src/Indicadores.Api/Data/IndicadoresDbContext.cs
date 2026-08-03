using Indicadores.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Indicadores.Api.Data;

public class IndicadoresDbContext(DbContextOptions<IndicadoresDbContext> options) : DbContext(options)
{
    public DbSet<Indicador> Indicadores => Set<Indicador>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var indicador = modelBuilder.Entity<Indicador>();

        indicador.ToTable("indicadores");
        indicador.HasKey(e => e.Id);

        indicador.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        indicador.Property(e => e.Categoria)
            .IsRequired()
            .HasMaxLength(100);

        indicador.Property(e => e.Valor)
            .HasPrecision(18, 4);

        indicador.Property(e => e.Unidad)
            .IsRequired()
            .HasMaxLength(50);

        indicador.Property(e => e.Origen)
            .HasMaxLength(100);

        indicador.HasIndex(e => e.Categoria);
        indicador.HasIndex(e => e.FechaMedicion);
    }
}
