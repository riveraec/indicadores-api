using Indicadores.Api.Exceptions;
using Indicadores.Api.Models.Dtos;
using Indicadores.Api.Models.Entities;
using Indicadores.Api.Repositories;

namespace Indicadores.Api.Services;

public class IndicadorService(IIndicadorRepository repository) : IIndicadorService
{
    public async Task<IReadOnlyList<IndicadorResponse>> ObtenerTodosAsync(
        IndicadorFiltro? filtro = null,
        CancellationToken cancellationToken = default)
    {
        var indicadores = await repository.FindAsync(
            filtro?.Categoria,
            filtro?.Desde,
            filtro?.Hasta,
            cancellationToken);

        return indicadores.Select(MapToResponse).ToList();
    }

    public async Task<IndicadorResponse> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var indicador = await ObtenerEntidadAsync(id, cancellationToken);
        return MapToResponse(indicador);
    }

    public async Task<IndicadorResponse> CrearAsync(
        IndicadorRequest request,
        CancellationToken cancellationToken = default)
    {
        var indicador = new Indicador();
        AplicarCambios(indicador, request);
        var creado = await repository.AddAsync(indicador, cancellationToken);
        return MapToResponse(creado);
    }

    public async Task<IndicadorResponse> ActualizarAsync(
        long id,
        IndicadorRequest request,
        CancellationToken cancellationToken = default)
    {
        var indicador = await ObtenerEntidadAsync(id, cancellationToken);
        AplicarCambios(indicador, request);
        await repository.UpdateAsync(indicador, cancellationToken);
        return MapToResponse(indicador);
    }

    public async Task EliminarAsync(long id, CancellationToken cancellationToken = default)
    {
        var indicador = await ObtenerEntidadAsync(id, cancellationToken);
        await repository.DeleteAsync(indicador, cancellationToken);
    }

    public async Task<IReadOnlyList<ResumenIndicadorResponse>> ObtenerResumenAsync(
        DateTime? desde = null,
        DateTime? hasta = null,
        CancellationToken cancellationToken = default)
    {
        var indicadores = await repository.FindAsync(
            categoria: null,
            desde: desde,
            hasta: hasta,
            cancellationToken);

        return indicadores
            .GroupBy(i => i.Categoria)
            .OrderBy(g => g.Key)
            .Select(g => new ResumenIndicadorResponse
            {
                Categoria = g.Key,
                Cantidad = g.Count(),
                Promedio = Math.Round(g.Average(i => i.Valor), 4),
                Min = g.Min(i => i.Valor),
                Max = g.Max(i => i.Valor)
            })
            .ToList();
    }

    private async Task<Indicador> ObtenerEntidadAsync(long id, CancellationToken cancellationToken)
    {
        var indicador = await repository.GetByIdAsync(id, cancellationToken);
        if (indicador is null)
        {
            throw new RecursoNoEncontradoException($"Indicador no encontrado con id: {id}");
        }

        return indicador;
    }

    private static void AplicarCambios(Indicador indicador, IndicadorRequest request)
    {
        indicador.Nombre = request.Nombre.Trim();
        indicador.Categoria = request.Categoria.Trim();
        indicador.Valor = request.Valor;
        indicador.Unidad = request.Unidad.Trim();
        indicador.FechaMedicion = NormalizarUtc(request.FechaMedicion);
        indicador.Origen = string.IsNullOrWhiteSpace(request.Origen) ? null : request.Origen.Trim();
    }

    private static IndicadorResponse MapToResponse(Indicador indicador) => new()
    {
        Id = indicador.Id,
        Nombre = indicador.Nombre,
        Categoria = indicador.Categoria,
        Valor = indicador.Valor,
        Unidad = indicador.Unidad,
        FechaMedicion = indicador.FechaMedicion,
        Origen = indicador.Origen
    };

    private static DateTime NormalizarUtc(DateTime fecha) =>
        fecha.Kind switch
        {
            DateTimeKind.Utc => fecha,
            DateTimeKind.Local => fecha.ToUniversalTime(),
            _ => DateTime.SpecifyKind(fecha, DateTimeKind.Utc)
        };
}
