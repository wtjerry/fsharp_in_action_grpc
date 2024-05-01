module FSharpInActionGrpc.GrpcGreeterService.GrpcGreeterServiceMetrics

open System.Collections.Generic
open System.Diagnostics.Metrics

[<Literal>]
let MetricName = "FSharpInActionGrpc.Lottery"

type GrpcGreeterServiceMetrics(meterFactory: IMeterFactory) =
    let lotteryWon =
        meterFactory.Create(MetricName).CreateCounter<int>($"{MetricName}.LotteryWon")

    member _.WinLottery(name, amount) =
        lotteryWon.Add(1, KeyValuePair("name", name), KeyValuePair("amount", amount))
