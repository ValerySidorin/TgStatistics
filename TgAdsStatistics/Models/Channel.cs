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
        public float OverallViews { get; set; }
        public float OverallMoneySpent { get; set; }
        public float OverallSubscribers { get; set; }
        public float AverageCostOfSubscriber { get; set; }
        public float OverallConvercy { get; set; }
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
