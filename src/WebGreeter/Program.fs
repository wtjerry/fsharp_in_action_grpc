namespace FSharpInActionGrpc.WebGreeter

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open OpenTelemetry.Exporter
open OpenTelemetry.Logs
open OpenTelemetry.Metrics
open OpenTelemetry.Resources
open OpenTelemetry.Trace
open Microsoft.Extensions.Logging
open FSharpInActionGrpc.WebGreeter.WebGreeterMetrics

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        let serviceName = "WebGreeter"

        let exporterConfig =
            fun (e: OtlpExporterOptions) -> e.Endpoint = Uri("https://127.0.0.1:4317") |> ignore

        builder.Services.AddSingleton<WebGreeterMetrics>() |> ignore

        builder.Logging.AddOpenTelemetry(fun options ->
            options
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddOtlpExporter(exporterConfig)
            |> ignore)
        |> ignore

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(fun resource -> resource.AddService(serviceName) |> ignore)
            .WithTracing(fun tracing ->
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(exporterConfig)
                |> ignore)
            .WithMetrics(fun metrics ->
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter(MetricName)
                    .AddOtlpExporter(exporterConfig)
                |> ignore)
        |> ignore

        builder.Services.AddControllers() |> ignore

        builder.Services.AddEndpointsApiExplorer() |> ignore
        builder.Services.AddSwaggerGen() |> ignore

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseSwagger() |> ignore
            app.UseSwaggerUI() |> ignore

        app.UseHttpsRedirection() |> ignore

        app.UseAuthorization() |> ignore
        app.MapControllers() |> ignore

        app.Run()

        exitCode
