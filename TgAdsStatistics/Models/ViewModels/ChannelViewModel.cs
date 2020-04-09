using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models
{
    public class ChannelViewModel
    {
        [Required(ErrorMessage = "Введите название канала")]
        public string ChannelName { get; set; }
    }
}
