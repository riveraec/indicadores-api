using Indicadores.Api.Data;
using Indicadores.Api.Repositories;
using Indicadores.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Indicadores.Api.Tests;

internal static class ServicioTestHelper
{
    public static (IndicadoresDbContext Db, IndicadorService Service) CrearServicio()
    {
        var options = new DbContextOptionsBuilder<IndicadoresDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var db = new IndicadoresDbContext(options);
        var repository = new IndicadorRepository(db);
        var service = new IndicadorService(repository);
        return (db, service);
    }
}
