using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models.ViewModels
{
    public class EditViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
