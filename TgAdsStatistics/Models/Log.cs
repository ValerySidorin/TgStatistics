using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
        public string Body { get; set; }
    }
}
