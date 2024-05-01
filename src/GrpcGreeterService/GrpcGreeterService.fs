namespace FSharpInActionGrpc.GrpcGreeterService

open System
open System.Threading.Tasks
open FSharpInActionGrpc
open FSharpInActionGrpc.GrpcGreeterService.GrpcGreeterServiceMetrics
open Microsoft.Extensions.Logging
open Grpc.Core

type GrpcGreeterService(logger: ILogger<GrpcGreeterService>, metrics: GrpcGreeterServiceMetrics) =
    inherit Greeter.GreeterBase()

    override _.SayHello(request: HelloRequest, _: ServerCallContext) =
        logger.LogInformation("Grpc server was called.")

        let rng = Random()
        let hasWon = Convert.ToBoolean(rng.Next(0, 2))

        let msg =
          if hasWon then
            let amount = rng.Next(0, 1000000)
            metrics.WinLottery(request.Name, amount)
            "hello: " + request.Name + " you just won the lottery!"
          else
            "hello: " + request.Name


        let reply = HelloReply()
        reply.Message <- msg
        Task.FromResult(reply)
