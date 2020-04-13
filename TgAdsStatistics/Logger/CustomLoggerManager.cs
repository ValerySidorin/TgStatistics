using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TgAdsStatistics.Extensions;
using TgAdsStatistics.Models;

namespace TgAdsStatistics.Logger
{
    public class CustomLoggerManager
    {
        private IHttpContextAccessor httpContextAccessor;
        private ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
        });
        ILogger logger;

        public CustomLoggerManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            loggerFactory.AddFile("logger.txt");
            logger = loggerFactory.CreateLogger("FileLogger");
        }

        public Log CreateLog()
        {
            string requestbody;
            using (Stream stream = httpContextAccessor.HttpContext.Request.Body)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    requestbody = sr.ReadToEnd();
                }
            }
            Log log = new Log { DateTime = DateTime.Now, Body = requestbody, Host = httpContextAccessor.HttpContext.Request.Host.ToString(), Method = httpContextAccessor.HttpContext.Request.Method, Path = httpContextAccessor.HttpContext.Request.Path, Protocol = httpContextAccessor.HttpContext.Request.Protocol };
            logger.LogInformation($"{log.DateTime}, {log.Method}, {log.Path}, {log.Host}, {log.Protocol}");
            return log;
        }
    }
}
