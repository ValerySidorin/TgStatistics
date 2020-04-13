using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TgAdsStatistics.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Log> Logs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
