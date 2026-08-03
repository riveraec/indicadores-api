using Indicadores.Api.Exceptions;
using Indicadores.Api.Models.Dtos;
using Indicadores.Api.Models.Entities;

namespace Indicadores.Api.Tests;

public class IndicadorServiceTests
{
    [Fact]
    public async Task ObtenerResumenAsync_AgrupaPorCategoria_ConAgregadosCorrectos()
    {
        var (db, service) = ServicioTestHelper.CrearServicio();
        await using (db)
        {
            var fecha = new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Utc);

            db.Indicadores.AddRange(
                new Indicador
                {
                    Nombre = "L1",
                    Categoria = "latencia",
                    Valor = 100m,
                    Unidad = "ms",
                    FechaMedicion = fecha
                },
                new Indicador
                {
                    Nombre = "L2",
                    Categoria = "latencia",
                    Valor = 200m,
                    Unidad = "ms",
                    FechaMedicion = fecha.AddHours(1)
                },
                new Indicador
                {
                    Nombre = "D1",
                    Categoria = "disponibilidad",
                    Valor = 99.5m,
                    Unidad = "%",
                    FechaMedicion = fecha.AddHours(2)
                });
            await db.SaveChangesAsync();

            var resumen = await service.ObtenerResumenAsync();

            Assert.Equal(2, resumen.Count);

            var latencia = Assert.Single(resumen, r => r.Categoria == "latencia");
            Assert.Equal(2, latencia.Cantidad);
            Assert.Equal(150m, latencia.Promedio);
            Assert.Equal(100m, latencia.Min);
            Assert.Equal(200m, latencia.Max);

            var disponibilidad = Assert.Single(resumen, r => r.Categoria == "disponibilidad");
            Assert.Equal(1, disponibilidad.Cantidad);
            Assert.Equal(99.5m, disponibilidad.Promedio);
            Assert.Equal(99.5m, disponibilidad.Min);
            Assert.Equal(99.5m, disponibilidad.Max);
        }
    }

    [Fact]
    public async Task ObtenerResumenAsync_ConRangoDeFechas_FiltraMediciones()
    {
        var (db, service) = ServicioTestHelper.CrearServicio();
        await using (db)
        {
            db.Indicadores.AddRange(
                new Indicador
                {
                    Nombre = "Dentro",
                    Categoria = "latencia",
                    Valor = 50m,
                    Unidad = "ms",
                    FechaMedicion = new DateTime(2026, 8, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Indicador
                {
                    Nombre = "Fuera",
                    Categoria = "latencia",
                    Valor = 999m,
                    Unidad = "ms",
                    FechaMedicion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                });
            await db.SaveChangesAsync();

            var desde = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc);
            var hasta = new DateTime(2026, 8, 31, 23, 59, 59, DateTimeKind.Utc);

            var resumen = await service.ObtenerResumenAsync(desde, hasta);

            var latencia = Assert.Single(resumen);
            Assert.Equal("latencia", latencia.Categoria);
            Assert.Equal(1, latencia.Cantidad);
            Assert.Equal(50m, latencia.Promedio);
        }
    }

    [Fact]
    public async Task ObtenerPorIdAsync_IdInexistente_LanzaRecursoNoEncontrado()
    {
        var (db, service) = ServicioTestHelper.CrearServicio();
        await using (db)
        {
            var ex = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
                () => service.ObtenerPorIdAsync(99999));

            Assert.Contains("99999", ex.Message);
        }
    }

    [Fact]
    public async Task CrearAsync_PersisteYDevuelveResponseConId()
    {
        var (db, service) = ServicioTestHelper.CrearServicio();
        await using (db)
        {
            var request = new IndicadorRequest
            {
                Nombre = "  Latencia p95  ",
                Categoria = " latencia ",
                Valor = 120.5m,
                Unidad = " ms ",
                FechaMedicion = new DateTime(2026, 8, 2, 18, 0, 0, DateTimeKind.Utc),
                Origen = "  monitor  "
            };

            var creado = await service.CrearAsync(request);

            Assert.True(creado.Id > 0);
            Assert.Equal("Latencia p95", creado.Nombre);
            Assert.Equal("latencia", creado.Categoria);
            Assert.Equal(120.5m, creado.Valor);
            Assert.Equal("ms", creado.Unidad);
            Assert.Equal("monitor", creado.Origen);
            Assert.Equal(DateTimeKind.Utc, creado.FechaMedicion.Kind);

            var desdeDb = await service.ObtenerPorIdAsync(creado.Id);
            Assert.Equal(creado.Id, desdeDb.Id);
            Assert.Equal(creado.Nombre, desdeDb.Nombre);
        }
    }

    [Fact]
    public async Task ObtenerTodosAsync_ConFiltroCategoria_SoloDevuelveEsaCategoria()
    {
        var (db, service) = ServicioTestHelper.CrearServicio();
        await using (db)
        {
            var fecha = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc);
            db.Indicadores.AddRange(
                new Indicador
                {
                    Nombre = "A",
                    Categoria = "latencia",
                    Valor = 1m,
                    Unidad = "ms",
                    FechaMedicion = fecha
                },
                new Indicador
                {
                    Nombre = "B",
                    Categoria = "errores",
                    Valor = 2m,
                    Unidad = "%",
                    FechaMedicion = fecha
                });
            await db.SaveChangesAsync();

            var lista = await service.ObtenerTodosAsync(new IndicadorFiltro { Categoria = "latencia" });

            var item = Assert.Single(lista);
            Assert.Equal("latencia", item.Categoria);
            Assert.Equal("A", item.Nombre);
        }
    }
}
