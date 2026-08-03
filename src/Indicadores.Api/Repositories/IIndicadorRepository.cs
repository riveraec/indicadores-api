using Indicadores.Api.Models.Entities;

namespace Indicadores.Api.Repositories;

public interface IIndicadorRepository
{
    Task<IReadOnlyList<Indicador>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Indicador?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Indicador>> FindAsync(
        string? categoria = null,
        DateTime? desde = null,
        DateTime? hasta = null,
        CancellationToken cancellationToken = default);

    Task<Indicador> AddAsync(Indicador indicador, CancellationToken cancellationToken = default);

    Task UpdateAsync(Indicador indicador, CancellationToken cancellationToken = default);

    Task DeleteAsync(Indicador indicador, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}
