using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Discord.WebSocket;
using Newtonsoft.Json;
using SatiriquesBot.Services.Subscription.Content;

namespace SatiriquesBot.Services.Subscription
{
    public class RedditSubscription : SubscriptionBase<RedditContent>
    {
        private readonly List<string> _linkCache = new();
        
        public RedditSubscription(DiscordSocketClient client, ulong guildId, ulong channelId, string url) 
            : base(client, guildId, channelId, url) { }

        public override async Task UpdateWatcherAsync(RedditContent content)
        {
            var newLinks = content.feed.entry.Select(x => x.link.Href).ToArray();

            var linkToPost = newLinks.Except(_linkCache);

            foreach (var link in linkToPost)
            {
                await ReplyAsync(link);
            }
            
            _linkCache.Clear();
            _linkCache.AddRange(newLinks);
        }

        public override RedditContent AdaptContent(string urlResponse)
        {
            var doc = new XmlDocument();
            doc.LoadXml(urlResponse);
            var json = JsonConvert.SerializeXmlNode(doc);
            return JsonConvert.DeserializeObject<RedditContent>(json);
        }
    }
}