namespace Indicadores.Api.Exceptions;

/// <summary>
/// Excepción de dominio cuando un recurso solicitado no existe.
/// Se traduce a HTTP 404 con ProblemDetails en <see cref="RecursoNoEncontradoExceptionHandler"/>.
/// </summary>
public class RecursoNoEncontradoException(string mensaje) : Exception(mensaje);
