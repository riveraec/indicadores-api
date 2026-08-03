using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Indicadores.Api.Exceptions;

/// <summary>
/// Traduce <see cref="RecursoNoEncontradoException"/> a HTTP 404 con ProblemDetails.
/// </summary>
public class RecursoNoEncontradoExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not RecursoNoEncontradoException noEncontrado)
        {
            return false;
        }

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Recurso no encontrado",
            Detail = noEncontrado.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
