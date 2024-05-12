using Microsoft.AspNetCore.HttpOverrides;
using Bc.CyberSec.Detection.Booster.Api;
using Bc.CyberSec.Detection.Booster.Api.Core.Application;
using Bc.CyberSec.Detection.Booster.Api.Core.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Core.Dto;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<StartUp>();
}

StartUp.Configure(builder.Configuration, builder.Services);

var app = builder.Build();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(async () =>
{
    var service = app.Services.GetService<IUseCaseSerializerService>();
    await service!.Save(await service.GetUseCases());
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPut("/api/uc/activate/{id}", (int id, IUseCaseHandlerService useCaseHandlerService) =>
{
    try
    {
        useCaseHandlerService.Activate($"UC{id}");
        return Results.Ok();
    }
    catch (ApplicationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("Handle")
.WithOpenApi()
.RequireAuthorization();

app.MapPut("/api/uc/deactivate/{id}", (int id, IUseCaseHandlerService useCaseHandlerService) =>
{
    try
    {

        useCaseHandlerService.Deactivate($"UC{id}");
        return Results.Ok();
    }
    catch (ApplicationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("Deactivate")
.WithOpenApi()
.RequireAuthorization();

app.MapPost("/api/uc/save", async (List<UseCaseCreateDto> useCases, IUseCaseSerializerService useCaseSerializerService) =>
{
    if (useCases.Count == 0)
        return Results.BadRequest("No use cases provided");

    try
    {
        await useCaseSerializerService.Save(useCases);
        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("Configure")
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/api/uc/active", async (IUseCaseSerializerService useCaseSerializerService) =>
{
    try
    {
        return Results.Ok((await useCaseSerializerService.GetUseCases()).Where(uc => uc.IsActive));
    }
    catch (ApplicationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("GetActive");

app.MapGet("/api/uc/inactive", async (IUseCaseSerializerService useCaseSerializerService) =>
{
    try
    {
        return Results.Ok((await useCaseSerializerService.GetUseCases()).Where(uc => !uc.IsActive));
    }
    catch (ApplicationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("GetInActive");

app.MapGet("/api/uc/save", (IUseCaseSerializerService useCaseSerializerService) =>
{
    try
    {
        return Results.Ok(useCaseSerializerService.WhenSerialized().ToString("dd-MM-yyyy-HH-mm"));
    }
    catch (ApplicationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("GetSerialization");

app.MapGet("/api/uc/all", async (IUseCaseSerializerService useCaseSerializerService) =>
    {
        try
        {
            return Results.Ok(await useCaseSerializerService.GetUseCases());
        }
        catch (ApplicationException e)
        {
            return Results.BadRequest(e.Message);
        }
    })
    .WithName("GetAll");

app.Run();
