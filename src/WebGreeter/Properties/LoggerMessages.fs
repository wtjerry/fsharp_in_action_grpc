module FSharpInActionGrpc.WebGreeter.LoggerMessages

open Microsoft.Extensions.Logging
let LogGotReplyFrom logger name =
    LoggerMessage
        .Define<string>(LogLevel.Information, 100, "Got a reply from grpc {name}")
        .Invoke(logger, name, null)
