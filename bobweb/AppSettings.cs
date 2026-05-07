using System;

namespace bobweb
{
    public class AppSettings
    {
        public string FileSaveLocation { get; set; }
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
        public string AccessLogPath { get; set; } = "logs/access.log";
        public string ErrorLogPath { get; set; } = "logs/errors.log";

        public string GetAccessLogPath()
        {
            return AccessLogPath;
        }

        public string GetErrorLogPath()
        {
            return ErrorLogPath;
        }
    }
}
