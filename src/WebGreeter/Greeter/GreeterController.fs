namespace WebGreeter.Controllers

open FSharpInActionGrpc
open Grpc.Net.Client
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("[controller]")>]
type GreeterController (logger : ILogger<GreeterController>) =
    inherit ControllerBase()

    let perfectlyRandomNames =
        [|
            "Joanne Ausburn"
            "Donovan Croushorn"
            "Rayford Hirtz"
            "Henry Nusbaum"
        |]

    [<HttpGet>]
    member _.Get() =
        let rng = System.Random()
        let name = perfectlyRandomNames[rng.Next(perfectlyRandomNames.Length)]

        logger.LogInformation("creating channel")

        let channel = GrpcChannel.ForAddress("https://localhost:5011")
        let client = Greeter.GreeterClient(channel)
        let helloRequest = HelloRequest()
        helloRequest.Name <- name

        logger.LogInformation("calling grpc service")

        task {
            logger.LogInformation("calling grpc service")
            let! reply = client.SayHelloAsync(helloRequest).ResponseAsync |> Async.AwaitTask
            let result = $"Got a reply from grpc: '{reply.Message}'"
            logger.LogInformation(result)

            channel.ShutdownAsync() |> Async.AwaitTask |> ignore
            logger.LogInformation("shutdown")

            return result
        }
