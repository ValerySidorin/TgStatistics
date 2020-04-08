using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TgAdsStatistics.Logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string filepath;

        public FileLoggerProvider(string filepath)
        {
            this.filepath = filepath;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(filepath);
        }

        public void Dispose()
        {
            
        }
    }
}
