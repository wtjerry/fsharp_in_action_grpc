module FSharpInActionGrpc.WebGreeter.WebGreeterMetrics

open System.Collections.Generic
open System.Diagnostics.Metrics

let [<Literal>] MetricName = "FSharpInActionGrpc.Greetings"

type WebGreeterMetrics(meterFactory: IMeterFactory) =
    let greetingsGiven =
        meterFactory
            .Create(MetricName)
            .CreateCounter<int>($"{MetricName}.GreetingsGiven")

    member _.GiveGreeting(name) =
        greetingsGiven.Add(1, KeyValuePair("name", name))
