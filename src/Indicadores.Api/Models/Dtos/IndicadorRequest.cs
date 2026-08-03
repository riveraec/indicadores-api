using System.ComponentModel.DataAnnotations;

namespace Indicadores.Api.Models.Dtos;

/// <summary>
/// DTO de entrada para crear o actualizar un indicador.
/// </summary>
public class IndicadorRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(200, ErrorMessage = "El nombre no puede superar 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria")]
    [MaxLength(100, ErrorMessage = "La categoría no puede superar 100 caracteres")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "El valor es obligatorio")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "La unidad es obligatoria")]
    [MaxLength(50, ErrorMessage = "La unidad no puede superar 50 caracteres")]
    public string Unidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de medición es obligatoria")]
    public DateTime FechaMedicion { get; set; }

    [MaxLength(100, ErrorMessage = "El origen no puede superar 100 caracteres")]
    public string? Origen { get; set; }
}
