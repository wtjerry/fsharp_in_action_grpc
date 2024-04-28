# sample grpc server and client with f#

## FAQ

untrusted certificates
: https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-8.0&tabs=visual-studio%2Clinux-ubuntu#ubuntu-trust-the-certificate-for-service-to-service-communication

swagger UI url
: https://localhost:5021/swagger/index.html

openTelemetry collector docker command
: docker run --rm --name otelCollector -p 127.0.0.1:55679:55679 -v ./config.yaml:/etc/otelcol/config.yaml -v ./otel_output/:/var/log/otel_output/ otel/opentelemetry-collector:0.99.0
