using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models
{
    public class PostViewModel
    {
        public int Views { get; set; }
        public int Subscribers { get; set; }
        public int Cost { get; set; }
        public int ChannelId { get; set; }
        public IEnumerable<SelectListItem> Channels { get; set; }
    }
}
