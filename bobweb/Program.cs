using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace bobweb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception excp)
            {
                TryLogStartupException(excp);
                Console.WriteLine(excp);
            }
        }

        /// <summary>
        /// Writes startup failures to the configured error log path when the host fails before ASP.NET begins serving.
        /// </summary>
        private static void TryLogStartupException(Exception excp)
        {
            string path = Environment.GetEnvironmentVariable("AppSettings__ErrorLogPath");
            if (string.IsNullOrWhiteSpace(path))
                path = Path.Combine("logs", "errors.log");

            string message = string.Format("[{0:yyyy-MM-dd HH:mm:ss zzz}] startup_exception\n{1}\n", DateTimeOffset.Now, excp);
            FileLogWriter.WriteLine(path, message.TrimEnd('\r', '\n'));
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
