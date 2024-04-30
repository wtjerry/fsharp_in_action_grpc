# sample grpc server and client with f#

## disclaimer
THIS IS NOT INTENDED FOR PRODUCTION USE!
Among other things the following considerations need to be made before using this in production:
- certificate handling: this solution currently used unsecured grpc / http
- persistent volumes: this solution doesn't persist any data, eg jaeger database
- WebGreeter, GrpcGreeterService: those are just playground services to produce traces, logs and metrics (and to improve my f# skills ;)

## idea

### problem
This repo contains a possible solution to the fowlloing problem with centralized logging:
A system gets deployed to multiple locations, all without internet connection. (ie. running autonomously)
Once something goes wrong during operation, the logs/metrics of those systems should still be somehow accessible to 2nd/3rd level support.

### solution
One solution would be to deploy a full blown centralized logging backend stack. (eg. jaeger, ELK, etc)
This comes with the drawbacks of complexity, and possible license costs.

The solution shown here is to use openTelemetry, so that the application doesnt need to know about the specific sinks.
But use an intermediary file storage mechanism so that we can ship those logs to 2nd/3rd level support:
1. emit logs and metrics via openTelemetry protocol
2. sink them to a file via otelCollector
3. ship them to 2nd/3rd level support
4. read file(s) and emit logs/metrics via openTelemetry protocol
5. sink into jaeger, ELK stack, graphana, whatever

### example with this repository

generate logs
1. start the WebGreeter and GrpcGreeterService programs
2. start the otelCollectorToDisk containers
3. generate some logs&metrics (eg via swagger UI)
4. observe that metrics/traces/logs are generated on disk

consume logs
1. from within src/openTelemetryCollector_read_from_disk/ run ```docker-compose up```
2. copy the previously generated traces to the directory watched by otelCollector (ie. src/openTelemetryCollector_read_from_disk/otel_input/)
3. observe that:
    1. jaeger has imported traces (localhost:16686)
    2. prometheus has imported metrics (localhost:9090)


## FAQ

untrusted certificates
: https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-8.0&tabs=visual-studio%2Clinux-ubuntu#ubuntu-trust-the-certificate-for-service-to-service-communication

swagger UI url
: https://localhost:5021/swagger/index.html

openTelemetry collector to file docker command (note no contrib)
: ```docker run --rm --name otelCollectorToDisk -p 127.0.0.1:55679:55679 -p 4317:4317 -p 4318:4318 -v ./config_save_to_disk.yaml:/etc/otelcol/config.yaml -v ./otel_output/:/var/log/otel_output/ otel/opentelemetry-collector:0.99.0```

