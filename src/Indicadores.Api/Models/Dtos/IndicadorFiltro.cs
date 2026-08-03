namespace Indicadores.Api.Models.Dtos;

/// <summary>
/// Filtros de consulta para listados de indicadores (query string).
/// </summary>
public class IndicadorFiltro
{
    public string? Categoria { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }
}
