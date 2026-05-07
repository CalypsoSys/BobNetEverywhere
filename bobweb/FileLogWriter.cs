using System;
using System.IO;
using System.Text;

namespace bobweb
{
    internal static class FileLogWriter
    {
        private static readonly object _sync = new object();

        /// <summary>
        /// Appends a single line to the configured log file and creates the directory when needed.
        /// </summary>
        public static void WriteLine(string path, string line)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            try
            {
                string fullPath = Path.GetFullPath(path);
                string directory = Path.GetDirectoryName(fullPath);
                if (string.IsNullOrWhiteSpace(directory) == false)
                    Directory.CreateDirectory(directory);

                lock (_sync)
                {
                    File.AppendAllText(fullPath, line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // do no harm
            }
        }

        /// <summary>
        /// Converts a possibly multi-line value into a single safe log field.
        /// </summary>
        public static string SanitizeSingleLine(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "-";

            return value.Replace("\r", " ").Replace("\n", " ").Trim();
        }
    }
}
