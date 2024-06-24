using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace QualvationLibrary
{

    public class CustomFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var istTime = TimeZoneInfo.ConvertTimeFromUtc(logEvent.Timestamp.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            output.Write("{");
            output.Write($"\"Timestamp\":\"{istTime:O}\","); // ISO 8601 format
            output.Write($"\"Level\":\"{logEvent.Level}\",");
            output.Write($"\"Message\":\"{logEvent.RenderMessage()}\",");

            if (logEvent.Exception != null)
            {
                output.Write($"\"Exception\":\"{logEvent.Exception}\"");
            }

            if (logEvent.Properties.Count > 0)
            {
                output.Write(",\"Properties\":{");

                bool isFirst = true;
                foreach (var property in logEvent.Properties)
                {
                    if (!isFirst) output.Write(",");
                    isFirst = false;

                    output.Write($"\"{property.Key}\":\"{property.Value}\"");
                }

                output.Write("}");
            }

            output.Write("}");
            output.WriteLine();
        }
    }

    public class ClientIpEnricherMiddleware(RequestDelegate next, ILogger<ClientIpEnricherMiddleware> logger)
    {

        public async Task Invoke(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            using (LogContext.PushProperty("ClientIP", clientIp))
            {
                logger.LogInformation($"Client IP: {clientIp}");

                var logFilePath = $"Logs/log-{clientIp ?? "unknown"}.txt";

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception} [ClientIp: {ClientIp}]")
                    .CreateLogger();

                await next(context);
            }

        }
    }
}
