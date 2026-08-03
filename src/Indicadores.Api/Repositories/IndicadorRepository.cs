using Indicadores.Api.Data;
using Indicadores.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Indicadores.Api.Repositories;

public class IndicadorRepository(IndicadoresDbContext db) : IIndicadorRepository
{
    public async Task<IReadOnlyList<Indicador>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Indicadores
            .AsNoTracking()
            .OrderByDescending(i => i.FechaMedicion)
            .ToListAsync(cancellationToken);
    }

    public async Task<Indicador?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await db.Indicadores
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Indicador>> FindAsync(
        string? categoria = null,
        DateTime? desde = null,
        DateTime? hasta = null,
        CancellationToken cancellationToken = default)
    {
        var query = db.Indicadores.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            query = query.Where(i => i.Categoria == categoria);
        }

        if (desde.HasValue)
        {
            query = query.Where(i => i.FechaMedicion >= desde.Value);
        }

        if (hasta.HasValue)
        {
            query = query.Where(i => i.FechaMedicion <= hasta.Value);
        }

        return await query
            .OrderByDescending(i => i.FechaMedicion)
            .ToListAsync(cancellationToken);
    }

    public async Task<Indicador> AddAsync(Indicador indicador, CancellationToken cancellationToken = default)
    {
        db.Indicadores.Add(indicador);
        await db.SaveChangesAsync(cancellationToken);
        return indicador;
    }

    public async Task UpdateAsync(Indicador indicador, CancellationToken cancellationToken = default)
    {
        db.Indicadores.Update(indicador);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Indicador indicador, CancellationToken cancellationToken = default)
    {
        db.Indicadores.Remove(indicador);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await db.Indicadores.AnyAsync(i => i.Id == id, cancellationToken);
    }
}
