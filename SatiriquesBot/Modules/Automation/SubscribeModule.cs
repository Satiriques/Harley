using System.Threading.Tasks;
using System.Web;
using Discord.Commands;
using SatiriquesBot.Services.Subscription;

namespace SatiriquesBot.Modules.Automation
{
    [Group("subscribe")]
    [Alias("sub")]
    public class SubscribeModule : ModuleBase<SocketCommandContext>
    {
        private readonly SubscriptionService _service;

        public SubscribeModule(SubscriptionService service)
        {
            _service = service;
        }
        
        [Command("reddit")]
        [Alias("r")]
        public async Task SubscribeAsync(string subreddit, string searchValue)
        {
            var url =
                $"https://old.reddit.com/r/{subreddit}/search.xml?sort=new&restrict_sr=on&q={HttpUtility.UrlEncode(searchValue)}";
            _service.AddSubscription<RedditSubscription>(Context, url);
        }
    }
}