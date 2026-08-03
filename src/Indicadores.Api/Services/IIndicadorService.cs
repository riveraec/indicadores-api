using Indicadores.Api.Models.Dtos;

namespace Indicadores.Api.Services;

public interface IIndicadorService
{
    Task<IReadOnlyList<IndicadorResponse>> ObtenerTodosAsync(
        IndicadorFiltro? filtro = null,
        CancellationToken cancellationToken = default);

    Task<IndicadorResponse> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IndicadorResponse> CrearAsync(IndicadorRequest request, CancellationToken cancellationToken = default);

    Task<IndicadorResponse> ActualizarAsync(
        long id,
        IndicadorRequest request,
        CancellationToken cancellationToken = default);

    Task EliminarAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ResumenIndicadorResponse>> ObtenerResumenAsync(
        DateTime? desde = null,
        DateTime? hasta = null,
        CancellationToken cancellationToken = default);
}
