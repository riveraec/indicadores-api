using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Indicadores.Api.Config;

/// <summary>
/// Configuración de la documentación OpenAPI / Swagger UI.
/// </summary>
public static class OpenApiConfig
{
    public static IServiceCollection AddIndicadoresSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Indicadores API",
                Version = "1.0.0",
                Description = "API REST de indicadores de desempeño — proyecto demo .NET 8 (EF Core + SQLite)."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        return services;
    }

    public static WebApplication UseIndicadoresSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Indicadores API v1");
            options.DocumentTitle = "Indicadores API";
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
