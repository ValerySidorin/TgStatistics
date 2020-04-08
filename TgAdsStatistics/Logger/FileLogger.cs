using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;

namespace TgAdsStatistics.Logger
{
    public class FileLogger : ILogger
    {
        private string filepath;
        private static object _lock = new object();

        public FileLogger(string filepath)
        {
            this.filepath = filepath;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(filepath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
