namespace FSharpInActionGrpc.GrpcGreeterService

open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection

module Program =
    [<EntryPoint>]
    let main args =
        let builder = Host.CreateApplicationBuilder(args)
        builder.Services.AddGrpc() |> ignore

        let app = builder.Build()
        app.Run()

        0
