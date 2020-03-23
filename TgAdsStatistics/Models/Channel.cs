using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgAdsStatistics.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public int NumberOfAdsPosted { get; set; }
        public int OverallViews { get; set; }
        public int OverallMoneySpent { get; set; }
        public int OverallSubscribers { get; set; }
        public int AverageCostOfSubscriber { get; set; }
        public int OverallConvercy { get; set; }
        public IEnumerable<Post> Posts { get; set; }

        public Channel()
        {

        }

        public Channel(int id)
        {
            Id = id;
        }
        public Channel(string channelname, int numberofadsposted, int overallviews, int overallmoneyspent, int overallsubscribers)
        {
            ChannelName = channelname;
            NumberOfAdsPosted = numberofadsposted;
            OverallViews = overallviews;
            OverallMoneySpent = overallmoneyspent;
            OverallSubscribers = overallsubscribers;
            AverageCostOfSubscriber = (OverallSubscribers == 0) ? 0 : (OverallMoneySpent / OverallSubscribers);
            OverallConvercy = (OverallSubscribers == 0) ? 0 : (OverallViews / OverallSubscribers);
        }
    }
}
