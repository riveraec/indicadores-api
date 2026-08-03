using Indicadores.Api.Models.Dtos;
using Indicadores.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Indicadores.Api.Controllers;

/// <summary>
/// Endpoints REST de indicadores bajo <c>/api/indicadores</c>.
/// </summary>
[ApiController]
[Route("api/indicadores")]
[Tags("Indicadores")]
[Produces("application/json")]
public class IndicadoresController(IIndicadorService indicadorService) : ControllerBase
{
    /// <summary>
    /// Lista indicadores. Filtros opcionales: categoria, desde, hasta.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<IndicadorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<IndicadorResponse>>> ObtenerTodos(
        [FromQuery] IndicadorFiltro filtro,
        CancellationToken cancellationToken)
    {
        var indicadores = await indicadorService.ObtenerTodosAsync(filtro, cancellationToken);
        return Ok(indicadores);
    }

    /// <summary>
    /// Resumen agregado por categoría (cantidad, promedio, min, max).
    /// Filtros opcionales: desde, hasta.
    /// </summary>
    [HttpGet("resumen")]
    [ProducesResponseType(typeof(IReadOnlyList<ResumenIndicadorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ResumenIndicadorResponse>>> ObtenerResumen(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        CancellationToken cancellationToken)
    {
        var resumen = await indicadorService.ObtenerResumenAsync(desde, hasta, cancellationToken);
        return Ok(resumen);
    }

    /// <summary>
    /// Obtiene un indicador por id.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(IndicadorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IndicadorResponse>> ObtenerPorId(
        long id,
        CancellationToken cancellationToken)
    {
        var indicador = await indicadorService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(indicador);
    }

    /// <summary>
    /// Crea un indicador.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(IndicadorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IndicadorResponse>> Crear(
        [FromBody] IndicadorRequest request,
        CancellationToken cancellationToken)
    {
        var creado = await indicadorService.CrearAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>
    /// Actualiza un indicador existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(IndicadorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IndicadorResponse>> Actualizar(
        long id,
        [FromBody] IndicadorRequest request,
        CancellationToken cancellationToken)
    {
        var actualizado = await indicadorService.ActualizarAsync(id, request, cancellationToken);
        return Ok(actualizado);
    }

    /// <summary>
    /// Elimina un indicador por id.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(long id, CancellationToken cancellationToken)
    {
        await indicadorService.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}
