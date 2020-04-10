using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TgAdsStatistics.Models;

namespace TgAdsStatistics.Extensions
{
    public class ContextAccessor
    {
        private IHttpContextAccessor httpContextAccessor;

        public ContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Log(ApplicationContext db, ILogger logger)
        {
            string requestbody;
            using (Stream stream = httpContextAccessor.HttpContext.Request.Body)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    requestbody = sr.ReadToEnd();
                }
            }
            Logs log = new Logs { DateTime = DateTime.Now, Body = requestbody, Host = httpContextAccessor.HttpContext.Request.Host.ToString(), Method = httpContextAccessor.HttpContext.Request.Method, Path = httpContextAccessor.HttpContext.Request.Path, Protocol = httpContextAccessor.HttpContext.Request.Protocol };
            db.Logs.Add(log);
            db.SaveChanges();
            logger.LogInformation($"{log.DateTime}, {log.Method}, {log.Path}, {log.Host}, {log.Protocol}, {log.Body}");
        }
    }
}
