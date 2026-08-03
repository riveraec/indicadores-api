namespace Indicadores.Api.Models.Dtos;

/// <summary>
/// Agregados de indicadores por categoría.
/// </summary>
public class ResumenIndicadorResponse
{
    public string Categoria { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public decimal Promedio { get; set; }

    public decimal Min { get; set; }

    public decimal Max { get; set; }
}
