using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Views { get; set; }
        public int Subscribers { get; set; }
        public int Cost { get; set; }
        public int Convercy { get; set; }
        public int SingleSubscriberCost { get; set; }
        public int? ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
