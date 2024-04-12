namespace FSharpInActionGrpc.GrpcGreeterService

open System.Threading.Tasks
open FSharpInActionGrpc
open Microsoft.Extensions.Logging
open Grpc.Core

type GrpcGreeterService (logger : ILogger<GrpcGreeterService>) =
    inherit Greeter.GreeterBase()

    override _.SayHello(request : HelloRequest, context : ServerCallContext) =
        logger.LogInformation("Grpc server was called.")
        let reply = HelloReply()
        reply.Message <- "hello: " + request.Name
        Task.FromResult(reply)
