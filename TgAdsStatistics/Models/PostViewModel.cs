using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models
{
    public class PostViewModel
    {
        [Required]
        public int Views { get; set; }
        [Required]
        public int Subscribers { get; set; }
        [Required]
        public int Cost { get; set; }
        public Channel Channel { get; set; }
        public int ChannelId { get; set; }
    }
}
