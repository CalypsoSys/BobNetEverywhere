using bobweb;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace bobweb.tests;

public class AccessLogMiddlewareTests
{
    [Fact]
    public async Task InvokeWritesAccessLogLineToConfiguredPath()
    {
        string tempRoot = Path.Combine(Path.GetTempPath(), $"bobweb-access-log-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempRoot);

        try
        {
            string logPath = Path.Combine(tempRoot, "access.log");
            var middleware = new AccessLogMiddleware(
                async context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength = 42;
                    await Task.CompletedTask;
                },
                Options.Create(new AppSettings
                {
                    AccessLogPath = logPath
                }));

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/api/items/my_list";
            context.Request.QueryString = new QueryString("?id=1");
            context.Request.Protocol = "HTTP/1.1";
            context.Request.Headers.Referer = "https://example.test/";
            context.Request.Headers.UserAgent = "bobweb-tests";
            context.Request.Headers["CF-Connecting-IP"] = "203.0.113.50";

            await middleware.Invoke(context);

            string contents = File.ReadAllText(logPath);
            Assert.Contains("203.0.113.50", contents);
            Assert.Contains("GET /api/items/my_list?id=1 HTTP/1.1", contents);
            Assert.Contains("\"https://example.test/\"", contents);
            Assert.Contains("\"bobweb-tests\"", contents);
        }
        finally
        {
            if (Directory.Exists(tempRoot))
                Directory.Delete(tempRoot, true);
        }
    }
}
