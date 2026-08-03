namespace Indicadores.Api.Models.Entities;

public class Indicador
{
    public long Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Unidad { get; set; } = string.Empty;

    public DateTime FechaMedicion { get; set; }

    public string? Origen { get; set; }
}
