namespace Indicadores.Api.Models.Dtos;

/// <summary>
/// DTO de salida de un indicador.
/// </summary>
public class IndicadorResponse
{
    public long Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Unidad { get; set; } = string.Empty;

    public DateTime FechaMedicion { get; set; }

    public string? Origen { get; set; }
}
