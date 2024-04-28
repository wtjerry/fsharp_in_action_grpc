# sample grpc server and client with f#

## idea
1. emit logs and metrics via openTelemetry protocol
2. sink them to a file via otelCollector
3. ship them to 2nd/3rd level support. (The system emitting the logs/metrics might be standalone -> no internet access)
4. read file(s) and emit logs/metrics via openTelemetry protocol
5. sink into jaeger, ELK stack, graphana, whatever

## FAQ

untrusted certificates
: https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-8.0&tabs=visual-studio%2Clinux-ubuntu#ubuntu-trust-the-certificate-for-service-to-service-communication

swagger UI url
: https://localhost:5021/swagger/index.html

openTelemetry collector to file docker command (note no contrib)
: docker run --rm --name otelCollectorToDisk -p 127.0.0.1:55679:55679 -p 4317:4317 -p 4318:4318 -v ./config_save_to_disk.yaml:/etc/otelcol/config.yaml -v ./otel_output/:/var/log/otel_output/ otel/opentelemetry-collector:0.99.0

openTelemetry collector from file docker command (note contrib)
: docker run --rm --name otelCollectorFromDisk -p 127.0.0.1:55679:55679 -v ./config_read_from_disk.yaml:/etc/otelcol-contrib/config.yaml -v ./otel_output/:/var/log/otel_output/ otel/opentelemetry-collector-contrib:0.99.0

jaeger docker command
: docker run --rm --name jaegerTest -p 16686:16686 -p 4317:4317 -p 4318:4318 jaegertracing/all-in-one:1.56

