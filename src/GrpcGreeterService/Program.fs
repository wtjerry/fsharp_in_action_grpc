namespace FSharpInActionGrpc.GrpcGreeterService

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddGrpc().AddJsonTranscoding() |> ignore

        builder.Services.AddGrpcSwagger() |> ignore
        builder.Services.AddSwaggerGen() |> ignore

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseSwagger() |> ignore
            app.UseSwaggerUI() |> ignore

        app.MapGrpcService<GrpcGreeterService>() |> ignore
        app.Run()

        0
