using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgAdsStatistics.Models;
using TgAdsStatistics.Logger;
using System.IO;

namespace TgAdsStatistics.Extensions
{
    public static class ApplicationExtensions
    {
        public static Channel Form(this Channel channel, Post post)
        {
            channel.NumberOfAdsPosted++;
            channel.OverallMoneySpent += post.Cost;
            channel.OverallViews += post.Views;
            channel.OverallSubscribers += post.Subscribers;
            channel.AverageCostOfSubscriber = (channel.OverallSubscribers == 0) ? 0 : (channel.OverallMoneySpent / channel.OverallSubscribers);
            channel.OverallConvercy = (channel.OverallSubscribers == 0) ? 0 : (channel.OverallViews / channel.OverallSubscribers);

            return channel;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), filename);
            loggerFactory.AddProvider(new FileLoggerProvider(path));
            return loggerFactory;
        }
    }
}
