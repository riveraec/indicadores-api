using Indicadores.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Indicadores.Api.Data;

/// <summary>
/// Carga datos de demo si la tabla está vacía (idempotente).
/// </summary>
public static class IndicadoresSeeder
{
    public static async Task SeedAsync(IndicadoresDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Indicadores.AnyAsync(cancellationToken))
        {
            return;
        }

        var baseFecha = new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Utc);

        var indicadores = new List<Indicador>
        {
            new()
            {
                Nombre = "Latencia p95 API pagos",
                Categoria = "latencia",
                Valor = 120.5m,
                Unidad = "ms",
                FechaMedicion = baseFecha,
                Origen = "monitor-pagos"
            },
            new()
            {
                Nombre = "Latencia p99 API pagos",
                Categoria = "latencia",
                Valor = 245.0m,
                Unidad = "ms",
                FechaMedicion = baseFecha.AddHours(1),
                Origen = "monitor-pagos"
            },
            new()
            {
                Nombre = "Latencia p95 API clientes",
                Categoria = "latencia",
                Valor = 88.2m,
                Unidad = "ms",
                FechaMedicion = baseFecha.AddHours(2),
                Origen = "monitor-clientes"
            },
            new()
            {
                Nombre = "Disponibilidad API pagos",
                Categoria = "disponibilidad",
                Valor = 99.95m,
                Unidad = "%",
                FechaMedicion = baseFecha.AddDays(1),
                Origen = "slo-pagos"
            },
            new()
            {
                Nombre = "Disponibilidad API clientes",
                Categoria = "disponibilidad",
                Valor = 99.80m,
                Unidad = "%",
                FechaMedicion = baseFecha.AddDays(1).AddHours(2),
                Origen = "slo-clientes"
            },
            new()
            {
                Nombre = "Throughput API pagos",
                Categoria = "throughput",
                Valor = 1540m,
                Unidad = "req/s",
                FechaMedicion = baseFecha.AddDays(2),
                Origen = "monitor-pagos"
            },
            new()
            {
                Nombre = "Throughput API clientes",
                Categoria = "throughput",
                Valor = 920m,
                Unidad = "req/s",
                FechaMedicion = baseFecha.AddDays(2).AddHours(3),
                Origen = "monitor-clientes"
            },
            new()
            {
                Nombre = "Errores 5xx API pagos",
                Categoria = "errores",
                Valor = 0.12m,
                Unidad = "%",
                FechaMedicion = baseFecha.AddDays(3),
                Origen = "monitor-pagos"
            },
            new()
            {
                Nombre = "Errores 5xx API clientes",
                Categoria = "errores",
                Valor = 0.35m,
                Unidad = "%",
                FechaMedicion = baseFecha.AddDays(3).AddHours(1),
                Origen = "monitor-clientes"
            },
            new()
            {
                Nombre = "Tiempo cola ETL reportes",
                Categoria = "batch",
                Valor = 42m,
                Unidad = "min",
                FechaMedicion = baseFecha.AddDays(4),
                Origen = "etl-reportes"
            }
        };

        db.Indicadores.AddRange(indicadores);
        await db.SaveChangesAsync(cancellationToken);
    }
}
