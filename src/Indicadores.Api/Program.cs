using Indicadores.Api.Config;
using Indicadores.Api.Data;
using Indicadores.Api.Exceptions;
using Indicadores.Api.Repositories;
using Indicadores.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddIndicadoresSwagger();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<RecursoNoEncontradoExceptionHandler>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors.Select(err =>
                $"{e.Key}: {err.ErrorMessage}"))
            .ToList();

        var problem = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "La solicitud contiene datos inválidos",
            Detail = "Revisa los campos marcados en 'errors'.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = context.HttpContext.Request.Path
        };
        problem.Extensions["mensajes"] = errors;

        return new BadRequestObjectResult(problem);
    };
});

builder.Services.AddDbContext<IndicadoresDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Indicadores")));

builder.Services.AddScoped<IIndicadorRepository, IndicadorRepository>();
builder.Services.AddScoped<IIndicadorService, IndicadorService>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<IndicadoresDbContext>("sqlite");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IndicadoresDbContext>();
    db.Database.EnsureCreated();
    await IndicadoresSeeder.SeedAsync(db);
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseIndicadoresSwagger();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
