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
        public float Views { get; set; }
        public float Subscribers { get; set; }
        public float Cost { get; set; }
        public float Convercy { get; set; }
        public float SingleSubscriberCost { get; set; }
        public int? ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
