using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace bobweb
{
    public class AccessLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _settings;

        public AccessLogMiddleware(RequestDelegate next, IOptions<AppSettings> options)
        {
            _next = next;
            _settings = options.Value ?? new AppSettings();
        }

        /// <summary>
        /// Logs each request in a combined-log-style single line after the pipeline has produced a response.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                string remoteIp = FileLogWriter.SanitizeSingleLine(GetRemoteIp(context));
                string method = FileLogWriter.SanitizeSingleLine(context.Request.Method);
                string path = FileLogWriter.SanitizeSingleLine(context.Request.Path + context.Request.QueryString);
                string protocol = FileLogWriter.SanitizeSingleLine(context.Request.Protocol);
                string referer = FileLogWriter.SanitizeSingleLine(context.Request.Headers.Referer.ToString());
                string userAgent = FileLogWriter.SanitizeSingleLine(context.Request.Headers.UserAgent.ToString());
                string contentLength = context.Response.ContentLength.HasValue ? context.Response.ContentLength.Value.ToString() : "-";

                string line = string.Format(
                    "{0} - - [{1:dd/MMM/yyyy:HH:mm:ss zzz}] \"{2} {3} {4}\" {5} {6} \"{7}\" \"{8}\" {9}ms",
                    remoteIp,
                    DateTimeOffset.Now,
                    method,
                    path,
                    protocol,
                    context.Response.StatusCode,
                    contentLength,
                    referer,
                    userAgent,
                    stopwatch.ElapsedMilliseconds);

                FileLogWriter.WriteLine(_settings.GetAccessLogPath(), line);
            }
        }

        /// <summary>
        /// Prefers the original client IP forwarded by Cloudflare and falls back to the direct socket peer.
        /// </summary>
        private static string GetRemoteIp(HttpContext context)
        {
            string cfConnectingIp = context.Request.Headers["CF-Connecting-IP"].ToString();
            if (string.IsNullOrWhiteSpace(cfConnectingIp) == false)
                return cfConnectingIp;

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}
