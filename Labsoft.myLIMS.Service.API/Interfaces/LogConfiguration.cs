using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;

namespace Interfaces
{
    public static class LogConfiguration
    {
        public static void AddSerilogLabsoftApplicationLog(
            IServiceCollection services, IConfiguration configuration, string environment)
        {
            var jsonFormatter = new JsonFormatter(closingDelimiter: null, renderMessage: true, formatProvider: null);
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ApplicationName", "Labsoft.myLIMSMobile.Api")
                .Enrich.WithProperty("Environment", environment)
                .MinimumLevel.Error()
                .WriteTo.RabbitMQ(
                    hostname: configuration["ApplicationLog_HostName"],
                    port: int. TryParse (configuration["ApplicationLog_Port"], out int rabbitMQPort) ? rabbitMQPort : 15672,
                    vHost: configuration["ApplicationLog_vHost"],
                    username: configuration["ApplicationLog_UserName"],
                    password: configuration["ApplicationLog_Password"],
                    exchange: configuration["ApplicationLog_Exchange"],
                    routeKey: configuration["ApplicationLog_RoutingKey"],
                    exchangeType: "direct",
                    deliveryMode: RabbitMQDeliveryMode.NonDurable,
                    formatter: jsonFormatter)
                .WriteTo.Console()
                .CreateLogger();
            services.AddLogging(lb => lb.AddSerilog(logger));
        }

        public static void CreateLogSender(HttpRequest request, Microsoft.Extensions.Logging.ILogger logger, Exception? ex) {
            if(ex != null)
            {
                var endpoint = $"{request.Method} {request.Path}{request.QueryString}";
                var message = $"An error occurred on endpoint {endpoint} ended run with system error.";

                logger.LogError(ex, message);
            }
        }
    }
}