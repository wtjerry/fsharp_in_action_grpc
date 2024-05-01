namespace FSharpInActionGrpc.GrpcGreeterService

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open OpenTelemetry.Logs
open OpenTelemetry.Resources
open OpenTelemetry.Exporter
open OpenTelemetry.Metrics
open OpenTelemetry.Trace
open FSharpInActionGrpc.GrpcGreeterService.GrpcGreeterServiceMetrics

module Program =
    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddGrpc().AddJsonTranscoding() |> ignore

        builder.Services.AddGrpcSwagger() |> ignore
        builder.Services.AddSwaggerGen() |> ignore

        let serviceName = "GrpcGreeterService"

        let exporterConfig =
            fun (e: OtlpExporterOptions) -> e.Endpoint = Uri("https://127.0.0.1:4317") |> ignore

        builder.Services.AddSingleton<GrpcGreeterServiceMetrics>() |> ignore

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

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseSwagger() |> ignore
            app.UseSwaggerUI() |> ignore

        app.MapGrpcService<GrpcGreeterService>() |> ignore
        app.Run()

        0
