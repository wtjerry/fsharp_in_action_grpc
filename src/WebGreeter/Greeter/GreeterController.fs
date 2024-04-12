namespace WebGreeter.Controllers

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

        
        "hello"
